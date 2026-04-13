using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AirADV.Services;
using AirADV.Services.Localization;
using LibVLCSharp.Shared;
using NAudio.MediaFoundation;
using NAudio.Wave;

namespace AirADV.Forms
{
    /// <summary>
    /// Audio Editor con waveform reale (2 fasi), gain, editing distruttivo e cursor di playback.
    /// Supporta preview video per file video in modalità RadioTV.
    /// </summary>
    public class AudioEditorForm : Form
    {
        // ── Waveform data (2-phase) ──────────────────────────────────
        private float[] _originalWaveformData;
        private float[] _waveformData;
        private Bitmap _waveformBitmap;

        // ── NAudio (samples per editing distruttivo) ──────────────────
        private float[] _samples;
        private int _sampleRate;
        private int _channels;
        private WaveFormat _waveFormat;

        // ── NAudio (playback) ────────────────────────────────────────
        private WaveOutEvent _waveOut;
        private WaveStream _audioReader;
        private bool _isPlaying;

        // ── VLC (video preview) ──────────────────────────────────────
        private LibVLC _libVLC;
        private LibVLCSharp.Shared.MediaPlayer _vlcPlayer;
        private LibVLCSharp.WinForms.VideoView _videoView;

        // ── Stato ────────────────────────────────────────────────────
        private readonly string _filePath;
        private readonly bool _isVideo;
        private float _gainDb = 0f;

        // ── Selezione waveform (indici campioni) ──────────────────────
        private int _selStart = -1;
        private int _selEnd = -1;
        private bool _isDragging;
        private int _dragStartX;

        // ── Cursor di playback ────────────────────────────────────────
        private System.Windows.Forms.Timer _playbackTimer;
        private float _playbackPosition = 0f; // 0..1

        // ── Controlli UI ─────────────────────────────────────────────
        private Panel pnlToolbar;
        private PictureBox picWaveform;
        private Panel pnlVideo;
        private Panel pnlControls;
        private Label lblGain;
        private TrackBar trkGain;
        private Label lblGainValue;
        private Button btnPlay;
        private Button btnPause;
        private Button btnStop;
        private Button btnDeleteSel;
        private Button btnSave;
        private Button btnCancel;
        private Label lblStatus;

        private static readonly string[] VIDEO_EXTENSIONS =
            { ".mp4", ".avi", ".mkv", ".mov", ".wmv", ".flv", ".webm", ".m4v", ".mpg", ".mpeg" };

        public AudioEditorForm(string filePath)
        {
            _filePath = filePath;
            string ext = Path.GetExtension(filePath ?? "").ToLowerInvariant();
            bool isVideoFile = VIDEO_EXTENSIONS.Contains(ext);
            // Mostra preview video solo in modalità RadioTV
            _isVideo = isVideoFile && ConfigManager.StationMode == "Radio-TV";

            BuildUI();
            ApplyLanguage();

            this.KeyPreview = true;
            this.Load += AudioEditorForm_Load;
            this.FormClosing += AudioEditorForm_FormClosing;
            this.KeyDown += AudioEditorForm_KeyDown;
            this.Resize += (s, e) => RecreateWaveformBitmapWithBoost();
        }

        // ═══════════════════════════════════════════════════════════
        // COSTRUZIONE UI
        // ═══════════════════════════════════════════════════════════
        private void BuildUI()
        {
            this.Text = LanguageManager.Get("AudioEditor.Title", "✏️ Audio Editor");
            this.Size = new Size(920, 580);
            this.MinimumSize = new Size(720, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 9.5f);

            // ── Pannello controlli in basso ───────────────────────────
            pnlControls = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 56,
                BackColor = Color.FromArgb(37, 37, 38),
                Padding = new Padding(10, 8, 10, 8)
            };

            lblGain = new Label
            {
                Text = LanguageManager.Get("AudioEditor.LblGain", "Gain (dB):"),
                AutoSize = true,
                ForeColor = Color.White,
                Location = new Point(10, 18),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            trkGain = new TrackBar
            {
                Minimum = -20,
                Maximum = 20,
                Value = 0,
                TickFrequency = 4,
                SmallChange = 1,
                LargeChange = 4,
                Width = 200,
                Location = new Point(95, 10),
                BackColor = Color.FromArgb(37, 37, 38)
            };
            trkGain.ValueChanged += TrkGain_ValueChanged;

            lblGainValue = new Label
            {
                Text = "0 dB",
                AutoSize = true,
                ForeColor = Color.Lime,
                Location = new Point(305, 18),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            btnDeleteSel = MakeWideButton(
                LanguageManager.Get("AudioEditor.BtnDeleteSelection", "🗑️ Elimina Selezione"),
                Color.FromArgb(220, 53, 69), 210, 36);
            btnDeleteSel.Location = new Point(370, 10);
            btnDeleteSel.Click += BtnDeleteSel_Click;

            btnSave = MakeWideButton(
                LanguageManager.Get("AudioEditor.BtnSave", "💾 Salva"),
                Color.FromArgb(40, 167, 69), 110, 36);
            btnSave.Location = new Point(590, 10);
            btnSave.Click += BtnSave_Click;

            btnCancel = MakeWideButton(
                LanguageManager.Get("AudioEditor.BtnCancel", "✖ Annulla"),
                Color.FromArgb(108, 117, 125), 110, 36);
            btnCancel.Location = new Point(710, 10);
            btnCancel.Click += (s, e) => this.Close();

            lblStatus = new Label
            {
                Text = "",
                AutoSize = true,
                ForeColor = Color.Silver,
                Location = new Point(835, 18),
                Font = new Font("Segoe UI", 9f)
            };

            pnlControls.Controls.AddRange(new Control[]
                { lblGain, trkGain, lblGainValue, btnDeleteSel, btnSave, btnCancel, lblStatus });

            // ── Toolbar in alto con Play/Pause/Stop quadrati ──────────
            pnlToolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = Color.FromArgb(37, 37, 38),
                Padding = new Padding(8, 6, 8, 6)
            };

            btnPlay = MakeIconButton("▶", Color.FromArgb(40, 167, 69));
            btnPlay.Location = new Point(8, 6);
            btnPlay.Click += BtnPlay_Click;

            btnPause = MakeIconButton("❚❚", Color.FromArgb(255, 140, 0));
            btnPause.Location = new Point(56, 6);
            btnPause.Click += BtnPause_Click;

            btnStop = MakeIconButton("⏹", Color.FromArgb(220, 53, 69));
            btnStop.Location = new Point(104, 6);
            btnStop.Click += BtnStop_Click;

            pnlToolbar.Controls.AddRange(new Control[] { btnPlay, btnPause, btnStop });

            // ── Area centrale: waveform + eventuale preview video ─────
            picWaveform = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(10, 10, 10),
                Cursor = Cursors.Cross
            };
            picWaveform.Paint += PicWaveform_Paint;
            picWaveform.MouseDown += PicWaveform_MouseDown;
            picWaveform.MouseMove += PicWaveform_MouseMove;
            picWaveform.MouseUp += PicWaveform_MouseUp;
            this.Controls.Add(picWaveform);

            if (_isVideo)
            {
                pnlVideo = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 160,
                    BackColor = Color.Black
                };
                this.Controls.Add(pnlVideo);
            }

            this.Controls.Add(pnlToolbar);
            this.Controls.Add(pnlControls);

            // ── Timer playback cursor ─────────────────────────────────
            _playbackTimer = new System.Windows.Forms.Timer { Interval = 50 };
            _playbackTimer.Tick += PlaybackTimer_Tick;
        }

        private Button MakeIconButton(string icon, Color back)
        {
            var btn = new Button
            {
                Text = icon,
                Size = new Size(40, 40),
                BackColor = back,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private Button MakeWideButton(string text, Color back, int width, int height)
        {
            var btn = new Button
            {
                Text = text,
                Size = new Size(width, height),
                BackColor = back,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f)
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void ApplyLanguage()
        {
            this.Text = LanguageManager.Get("AudioEditor.Title", "✏️ Audio Editor");
            btnPlay.Text = LanguageManager.Get("AudioEditor.BtnPlay", "▶");
            btnPause.Text = LanguageManager.Get("AudioEditor.BtnPause", "❚❚");
            btnStop.Text = LanguageManager.Get("AudioEditor.BtnStop", "⏹");
            lblGain.Text = LanguageManager.Get("AudioEditor.LblGain", "Gain (dB):");
            btnDeleteSel.Text = LanguageManager.Get("AudioEditor.BtnDeleteSelection", "🗑️ Elimina Selezione");
            btnSave.Text = LanguageManager.Get("AudioEditor.BtnSave", "💾 Salva");
            btnCancel.Text = LanguageManager.Get("AudioEditor.BtnCancel", "✖ Annulla");
        }

        // ═══════════════════════════════════════════════════════════
        // LOAD
        // ═══════════════════════════════════════════════════════════
        private void AudioEditorForm_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_filePath) || !File.Exists(_filePath))
            {
                MessageBox.Show(
                    LanguageManager.Get("AudioEditor.NoFile", "Nessun file da modificare!"),
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            if (_isVideo)
                InitVideoPlayer();

            LoadWaveformAndSamples();
        }

        // ── Fase 1: quick preview 800 campioni + Fase 2: full 6000 campioni ──
        private void LoadWaveformAndSamples()
        {
            try
            {
                // Fase 1 — quick preview (800 punti)
                LoadWaveformPhase(800);
                RecreateWaveformBitmapWithBoost();

                // Fase 2 — alta risoluzione (6000 punti) in background
                Task.Run(() =>
                {
                    try
                    {
                        LoadWaveformPhase(6000);
                        this.Invoke(new Action(() =>
                        {
                            RecreateWaveformBitmapWithBoost();
                        }));
                    }
                    catch { }
                });

                // Carica tutti i campioni in memoria per editing distruttivo
                LoadAllSamples();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AudioEditor] Errore caricamento: {ex.Message}");
                if (lblStatus != null) lblStatus.Text = $"Errore: {ex.Message}";
            }
        }

        private void LoadWaveformPhase(int numPoints)
        {
            if (_isVideo)
            {
                LoadWaveformFromVideoSequential(numPoints);
                return;
            }

            using (var reader = new AudioFileReader(_filePath))
            {
                var format = reader.WaveFormat;
                long totalSamples = reader.Length / (format.BitsPerSample / 8);
                long samplesPerPoint = Math.Max(1, totalSamples / numPoints);
                long bytesPerPoint = samplesPerPoint * (format.BitsPerSample / 8);

                float[] quickData = new float[numPoints];
                var sampleProvider = reader.ToSampleProvider();
                float[] buffer = new float[8192];

                Parallel.For(0, numPoints, new ParallelOptions { MaxDegreeOfParallelism = 4 }, i =>
                {
                    try
                    {
                        long targetByte = (long)i * bytesPerPoint;
                        if (targetByte < reader.Length - (long)buffer.Length * (format.BitsPerSample / 8))
                        {
                            lock (reader)
                            {
                                reader.Position = targetByte;
                                int samplesRead = sampleProvider.Read(buffer, 0, buffer.Length);
                                float max = 0f;
                                for (int j = 0; j < samplesRead; j++)
                                {
                                    float sample = Math.Abs(buffer[j]);
                                    if (sample > max) max = sample;
                                }
                                quickData[i] = max;
                            }
                        }
                    }
                    catch { quickData[i] = 0f; }
                });

                _originalWaveformData = quickData;
                _waveformData = (float[])quickData.Clone();
            }
        }

        private void LoadWaveformFromVideoSequential(int numPoints)
        {
            try
            {
                using (var reader = new MediaFoundationReader(_filePath))
                {
                    var sampleProvider = reader.ToSampleProvider();
                    var allSamples = new List<float>();
                    float[] buffer = new float[4096];
                    int read;
                    while ((read = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        for (int i = 0; i < read; i++)
                            allSamples.Add(Math.Abs(buffer[i]));
                    }

                    float[] data = new float[numPoints];
                    int total = allSamples.Count;
                    for (int i = 0; i < numPoints; i++)
                    {
                        int start = (int)((long)i * total / numPoints);
                        int end = (int)((long)(i + 1) * total / numPoints);
                        end = Math.Max(end, start + 1);
                        end = Math.Min(end, total);
                        float max = 0f;
                        for (int j = start; j < end; j++)
                            if (allSamples[j] > max) max = allSamples[j];
                        data[i] = max;
                    }

                    _originalWaveformData = data;
                    _waveformData = (float[])data.Clone();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AudioEditor] Errore waveform video: {ex.Message}");
            }
        }

        private void LoadAllSamples()
        {
            try
            {
                if (_isVideo)
                {
                    using (var reader = new MediaFoundationReader(_filePath))
                    {
                        _sampleRate = reader.WaveFormat.SampleRate;
                        _channels = reader.WaveFormat.Channels;
                        _waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(_sampleRate, _channels);

                        var sampleProvider = reader.ToSampleProvider();
                        var allSamples = new List<float>();
                        float[] buffer = new float[_sampleRate * _channels];
                        int read;
                        while ((read = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            for (int i = 0; i < read; i++)
                                allSamples.Add(buffer[i]);
                        }
                        _samples = allSamples.ToArray();
                    }
                }
                else
                {
                    using (var reader = new AudioFileReader(_filePath))
                    {
                        _waveFormat = reader.WaveFormat;
                        _sampleRate = reader.WaveFormat.SampleRate;
                        _channels = reader.WaveFormat.Channels;

                        var allSamples = new List<float>();
                        float[] buffer = new float[_sampleRate * _channels];
                        int read;
                        while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            for (int i = 0; i < read; i++)
                                allSamples.Add(buffer[i]);
                        }
                        _samples = allSamples.ToArray();
                    }
                }

                if (lblStatus != null && _samples != null)
                    lblStatus.Text = $"{Path.GetFileName(_filePath)} | {_sampleRate} Hz | {_channels}ch | {(_samples.Length / _channels / _sampleRate):F1}s";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AudioEditor] Errore campioni: {ex.Message}");
            }
        }

        private void InitVideoPlayer()
        {
            try
            {
                Core.Initialize();
                _libVLC = new LibVLC("--no-video-title-show");
                _vlcPlayer = new LibVLCSharp.Shared.MediaPlayer(_libVLC);

                _videoView = new LibVLCSharp.WinForms.VideoView
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.Black
                };
                _videoView.MediaPlayer = _vlcPlayer;
                pnlVideo.Controls.Add(_videoView);

                var media = new Media(_libVLC, new Uri(_filePath));
                _vlcPlayer.Media = media;

                lblStatus.Text = $"📺 {Path.GetFileName(_filePath)}";
                this.Text = LanguageManager.Get("AudioEditor.VideoPreviewTitle", "📺 Preview Video")
                    + $" — {Path.GetFileName(_filePath)}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AudioEditor] Errore VLC init: {ex.Message}");
                lblStatus.Text = $"Errore video: {ex.Message}";
            }
        }

        // ═══════════════════════════════════════════════════════════
        // WAVEFORM BITMAP (RecreateWaveformBitmapWithBoost)
        // ═══════════════════════════════════════════════════════════
        private void RecreateWaveformBitmapWithBoost()
        {
            if (_originalWaveformData == null || _originalWaveformData.Length == 0) return;
            if (picWaveform == null) return;

            float linearGain = (float)Math.Pow(10.0, _gainDb / 20.0);
            _waveformData = new float[_originalWaveformData.Length];
            for (int i = 0; i < _originalWaveformData.Length; i++)
                _waveformData[i] = Math.Min(1.0f, _originalWaveformData[i] * linearGain);

            int width = Math.Max(1, picWaveform.Width);
            int height = Math.Max(1, picWaveform.Height > 0 ? picWaveform.Height : 350);

            _waveformBitmap?.Dispose();
            _waveformBitmap = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(_waveformBitmap))
            {
                g.Clear(Color.FromArgb(10, 10, 10));
                g.SmoothingMode = SmoothingMode.HighQuality;
                int midY = height / 2;

                for (int px = 0; px < width; px++)
                {
                    int sampleIdx = (int)((float)px / width * _waveformData.Length);
                    sampleIdx = Math.Max(0, Math.Min(sampleIdx, _waveformData.Length - 1));
                    float amplitude = _waveformData[sampleIdx] * (height / 2) * 0.98f;

                    Color colorTop = Color.FromArgb(0, 200, 120);
                    Color colorBottom = Color.FromArgb(0, 160, 90);

                    using (Pen penTop = new Pen(colorTop, 1.2f))
                    using (Pen penBottom = new Pen(colorBottom, 1.2f))
                    {
                        g.DrawLine(penTop, px, midY, px, midY - (int)amplitude);
                        g.DrawLine(penBottom, px, midY, px, midY + (int)amplitude);
                    }
                }

                // Linea centrale
                using (Pen penCenter = new Pen(Color.FromArgb(80, 80, 80), 1f))
                    g.DrawLine(penCenter, 0, midY, width, midY);
            }

            picWaveform?.Invalidate();
        }

        // ═══════════════════════════════════════════════════════════
        // WAVEFORM PAINT & MOUSE
        // ═══════════════════════════════════════════════════════════
        private void PicWaveform_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            int w = picWaveform.Width;
            int h = picWaveform.Height;

            // Disegna bitmap pre-renderizzata
            if (_waveformBitmap != null)
                g.DrawImage(_waveformBitmap, 0, 0);
            else
                g.Clear(Color.FromArgb(10, 10, 10));

            // Overlay selezione (rosso semi-trasparente)
            if (_selStart >= 0 && _selEnd > _selStart && _samples != null && _samples.Length > 0)
            {
                float x1 = (float)_selStart / _samples.Length * w;
                float x2 = (float)_selEnd / _samples.Length * w;
                using (var selBrush = new SolidBrush(Color.FromArgb(80, 220, 53, 69)))
                    g.FillRectangle(selBrush, x1, 0, x2 - x1, h);
                using (var selPen = new Pen(Color.FromArgb(220, 53, 69), 1f))
                {
                    g.DrawLine(selPen, x1, 0, x1, h);
                    g.DrawLine(selPen, x2, 0, x2, h);
                }
            }

            // Cursore di playback (linea bianca verticale)
            if (_isPlaying && _playbackPosition >= 0f)
            {
                int cursorX = (int)(_playbackPosition * w);
                using (var cursorPen = new Pen(Color.White, 1.5f))
                    g.DrawLine(cursorPen, cursorX, 0, cursorX, h);
            }
        }

        private void PicWaveform_MouseDown(object sender, MouseEventArgs e)
        {
            if (_samples == null || _samples.Length == 0) return;
            _isDragging = true;
            _dragStartX = e.X;
            _selStart = (int)((float)e.X / picWaveform.Width * _samples.Length);
            _selEnd = _selStart;
            picWaveform.Invalidate();
        }

        private void PicWaveform_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging || _samples == null) return;
            int sampleIdx = (int)((float)e.X / picWaveform.Width * _samples.Length);
            sampleIdx = Math.Max(0, Math.Min(_samples.Length, sampleIdx));
            if (e.X >= _dragStartX)
            {
                _selStart = (int)((float)_dragStartX / picWaveform.Width * _samples.Length);
                _selEnd = sampleIdx;
            }
            else
            {
                _selStart = sampleIdx;
                _selEnd = (int)((float)_dragStartX / picWaveform.Width * _samples.Length);
            }
            picWaveform.Invalidate();
        }

        private void PicWaveform_MouseUp(object sender, MouseEventArgs e)
        {
            _isDragging = false;
        }

        // ═══════════════════════════════════════════════════════════
        // GAIN
        // ═══════════════════════════════════════════════════════════
        private void TrkGain_ValueChanged(object sender, EventArgs e)
        {
            _gainDb = trkGain.Value;
            lblGainValue.Text = $"{_gainDb:+0;-0;0} dB";
            lblGainValue.ForeColor = _gainDb > 0 ? Color.Orange : _gainDb < 0 ? Color.DeepSkyBlue : Color.Lime;

            // Aggiorna volume live in preview audio
            if (_audioReader is AudioFileReader afeGain)
                afeGain.Volume = (float)Math.Pow(10.0, _gainDb / 20.0);  // AudioFileReader supports > 1.0
            else if (_waveOut != null)
                _waveOut.Volume = Math.Min(1.0f, Math.Max(0.0f, (float)Math.Pow(10.0, _gainDb / 20.0)));  // Clamp to [0, 1]

            // Preview visivo in tempo reale sulla waveform
            RecreateWaveformBitmapWithBoost();
        }

        // ═══════════════════════════════════════════════════════════
        // SPACEBAR SHORTCUT
        // ═══════════════════════════════════════════════════════════
        private void AudioEditorForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                if (_isPlaying)
                    BtnPause_Click(sender, EventArgs.Empty);
                else
                    BtnPlay_Click(sender, EventArgs.Empty);
            }
        }

        // ═══════════════════════════════════════════════════════════
        // PLAYBACK + TIMER CURSOR
        // ═══════════════════════════════════════════════════════════
        private void PlaybackTimer_Tick(object sender, EventArgs e)
        {
            if (_audioReader != null && _samples != null && _samples.Length > 0 && _isPlaying)
            {
                long pos = _audioReader.Position;
                long len = _audioReader.Length;
                _playbackPosition = len > 0 ? (float)pos / len : 0f;
                picWaveform?.Invalidate();
            }
            else
            {
                _playbackTimer.Stop();
                _playbackPosition = 0f;
                picWaveform?.Invalidate();
            }
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            try
            {
                StopAudio();

                if (_isVideo)
                    _audioReader = new MediaFoundationReader(_filePath);
                else
                    _audioReader = new AudioFileReader(_filePath);

                float vol = (float)Math.Pow(10.0, _gainDb / 20.0);
                if (_audioReader is AudioFileReader afePlay)
                    afePlay.Volume = vol;

                _waveOut = new WaveOutEvent();
                _waveOut.Volume = Math.Min(1.0f, Math.Max(0.0f, vol));
                _waveOut.Init(_audioReader);
                _waveOut.PlaybackStopped += (s, ev) =>
                {
                    _isPlaying = false;
                    _playbackTimer.Stop();
                    _playbackPosition = 0f;
                    var wf = picWaveform;
                    if (wf != null && wf.IsHandleCreated && !wf.IsDisposed)
                        wf.Invoke(new Action(() => wf.Invalidate()));
                };
                _waveOut.Play();
                _isPlaying = true;
                _playbackTimer.Start();
                lblStatus.Text = "▶ " + Path.GetFileName(_filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AudioEditor] Errore play: {ex.Message}");
            }
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            if (_waveOut != null && _isPlaying)
            {
                _waveOut.Pause();
                _isPlaying = false;
                _playbackTimer.Stop();
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            StopAudio();
        }

        private void StopAudio()
        {
            try
            {
                _playbackTimer?.Stop();
                _playbackPosition = 0f;

                if (_waveOut != null)
                {
                    _waveOut.Stop();
                    _waveOut.Dispose();
                    _waveOut = null;
                }
                if (_audioReader != null)
                {
                    _audioReader.Dispose();
                    _audioReader = null;
                }
                _isPlaying = false;
                picWaveform?.Invalidate();
            }
            catch { }
        }

        // ═══════════════════════════════════════════════════════════
        // DELETE SELECTION (editing distruttivo)
        // ═══════════════════════════════════════════════════════════
        private void BtnDeleteSel_Click(object sender, EventArgs e)
        {
            if (_samples == null || _samples.Length == 0) return;

            if (_selStart < 0 || _selEnd <= _selStart)
            {
                MessageBox.Show(
                    LanguageManager.Get("AudioEditor.NoSelection", "Seleziona una porzione del file da eliminare!"),
                    LanguageManager.Get("Common.Warning", "Attenzione"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            StopAudio();

            // Rimuovi campioni selezionati
            var before = _samples.Take(_selStart).ToArray();
            var after = _samples.Skip(_selEnd).ToArray();
            _samples = before.Concat(after).ToArray();
            _selStart = -1;
            _selEnd = -1;

            // Rigenera waveform dai campioni aggiornati
            RegenerateWaveformFromSamples();

            TimeSpan newDuration = TimeSpan.FromSeconds((double)_samples.Length / _channels / _sampleRate);
            lblStatus.Text = $"✂️ Selezione eliminata | Durata: {newDuration:mm\\:ss\\.ff}";
        }

        private void RegenerateWaveformFromSamples()
        {
            if (_samples == null || _samples.Length == 0) return;

            int numPoints = Math.Min(6000, _samples.Length);
            float[] data = new float[numPoints];
            int step = Math.Max(1, _samples.Length / numPoints);

            for (int i = 0; i < numPoints; i++)
            {
                int start = i * step;
                int end = Math.Min(start + step, _samples.Length);
                float max = 0f;
                for (int j = start; j < end; j++)
                {
                    float abs = Math.Abs(_samples[j]);
                    if (abs > max) max = abs;
                }
                data[i] = max;
            }

            _originalWaveformData = data;
            RecreateWaveformBitmapWithBoost();
        }

        // ═══════════════════════════════════════════════════════════
        // SAVE (editing distruttivo - sovrascrittura file)
        // ═══════════════════════════════════════════════════════════
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (_samples == null || _samples.Length == 0) return;

            var confirm = MessageBox.Show(
                LanguageManager.Get("AudioEditor.ConfirmSave", "Vuoi sovrascrivere il file originale con le modifiche?"),
                LanguageManager.Get("AudioEditor.ConfirmSaveTitle", "Conferma Salvataggio"),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                StopAudio();

                float gainLinear = (float)Math.Pow(10.0, _gainDb / 20.0);

                // Applica gain ai samples prima di salvare
                float[] outputSamples = _samples;
                if (_gainDb != 0)
                {
                    outputSamples = new float[_samples.Length];
                    for (int i = 0; i < _samples.Length; i++)
                        outputSamples[i] = Math.Max(-1f, Math.Min(1f, _samples[i] * gainLinear));
                }

                // Scrivi tramite file temporaneo e sostituisci l'originale
                string tempPath = _filePath + ".tmp";
                WriteSamplesToWav(tempPath, outputSamples);
                File.Delete(_filePath);
                File.Move(tempPath, _filePath);

                _samples = outputSamples;
                _gainDb = 0;
                trkGain.Value = 0;

                MessageBox.Show(
                    LanguageManager.Get("AudioEditor.SaveSuccess", "✅ File salvato con successo!"),
                    LanguageManager.Get("Common.Success", "Successo"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                lblStatus.Text = "💾 " + LanguageManager.Get("AudioEditor.SaveSuccess", "File salvato!");
                RegenerateWaveformFromSamples();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AudioEditor] Errore salvataggio: {ex.Message}");
                MessageBox.Show(
                    $"{LanguageManager.Get("AudioEditor.SaveError", "Errore salvataggio file")}:\n{ex.Message}",
                    LanguageManager.Get("Common.Error", "Errore"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WriteSamplesToWav(string outputPath, float[] samples)
        {
            var format = WaveFormat.CreateIeeeFloatWaveFormat(_sampleRate, _channels);
            using (var writer = new WaveFileWriter(outputPath, format))
            {
                writer.WriteSamples(samples, 0, samples.Length);
            }
        }

        // ═══════════════════════════════════════════════════════════
        // CLEANUP
        // ═══════════════════════════════════════════════════════════
        private void AudioEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopAudio();
            _waveformBitmap?.Dispose();
            try
            {
                _vlcPlayer?.Stop();
                _vlcPlayer?.Dispose();
                _libVLC?.Dispose();
            }
            catch { }
        }
    }
}
