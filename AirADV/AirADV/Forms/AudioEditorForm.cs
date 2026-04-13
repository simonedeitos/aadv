using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AirADV.Services;
using AirADV.Services.Localization;
using LibVLCSharp.Shared;
using NAudio.Wave;

namespace AirADV.Forms
{
    /// <summary>
    /// Audio Editor con waveform, gain e editing distruttivo.
    /// Supporta anche preview video per file video in modalità RadioTV.
    /// </summary>
    public class AudioEditorForm : Form
    {
        // ── NAudio ──────────────────────────────────────────────────
        private WaveOutEvent _waveOut;
        private AudioFileReader _audioReader;
        private float[] _samples;
        private int _sampleRate;
        private int _channels;
        private WaveFormat _waveFormat;
        private bool _isPlaying;

        // ── VLC (video) ─────────────────────────────────────────────
        private LibVLC _libVLC;
        private LibVLCSharp.Shared.MediaPlayer _vlcPlayer;
        private LibVLCSharp.WinForms.VideoView _videoView;

        // ── stato ────────────────────────────────────────────────────
        private readonly string _filePath;
        private readonly bool _isVideo;
        private float _gainDb = 0f;

        // selezione waveform (campioni)
        private int _selStart = -1;
        private int _selEnd = -1;
        private bool _isDragging;
        private int _dragStartX;

        // ── controlli UI ─────────────────────────────────────────────
        private PictureBox picWaveform;
        private Panel pnlVideo;
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
            _isVideo = VIDEO_EXTENSIONS.Contains(ext);

            BuildUI();
            ApplyLanguage();

            this.Load += AudioEditorForm_Load;
            this.FormClosing += AudioEditorForm_FormClosing;
        }

        // ═══════════════════════════════════════════════════════════
        // COSTRUZIONE UI
        // ═══════════════════════════════════════════════════════════
        private void BuildUI()
        {
            this.Text = LanguageManager.Get("AudioEditor.Title", "✏️ Audio Editor");
            this.Size = new Size(900, 560);
            this.MinimumSize = new Size(700, 480);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 9.5f);

            // ── waveform / video area ─────────────────────────────
            if (_isVideo)
            {
                pnlVideo = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 320,
                    BackColor = Color.Black
                };
                this.Controls.Add(pnlVideo);
            }
            else
            {
                picWaveform = new PictureBox
                {
                    Dock = DockStyle.Top,
                    Height = 280,
                    BackColor = Color.FromArgb(30, 30, 30),
                    Cursor = Cursors.Cross
                };
                picWaveform.Paint += PicWaveform_Paint;
                picWaveform.MouseDown += PicWaveform_MouseDown;
                picWaveform.MouseMove += PicWaveform_MouseMove;
                picWaveform.MouseUp += PicWaveform_MouseUp;
                this.Controls.Add(picWaveform);
            }

            // ── gain row ─────────────────────────────────────────
            var pnlGain = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = Color.FromArgb(37, 37, 38),
                Padding = new Padding(10, 6, 10, 0)
            };

            lblGain = new Label
            {
                Text = LanguageManager.Get("AudioEditor.LblGain", "Gain (dB):"),
                AutoSize = true,
                ForeColor = Color.White,
                Location = new Point(10, 10),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            trkGain = new TrackBar
            {
                Minimum = -12,
                Maximum = 12,
                Value = 0,
                TickFrequency = 2,
                SmallChange = 1,
                LargeChange = 2,
                Width = 200,
                Location = new Point(100, 6),
                BackColor = Color.FromArgb(37, 37, 38)
            };
            trkGain.ValueChanged += TrkGain_ValueChanged;

            lblGainValue = new Label
            {
                Text = "0 dB",
                AutoSize = true,
                ForeColor = Color.Lime,
                Location = new Point(310, 10),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            pnlGain.Controls.AddRange(new Control[] { lblGain, trkGain, lblGainValue });
            this.Controls.Add(pnlGain);

            // ── bottom button row ──────────────────────────────────
            var pnlButtons = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.FromArgb(37, 37, 38),
                Padding = new Padding(10, 8, 10, 8)
            };

            btnPlay = MakeButton("▶ Play", Color.FromArgb(0, 120, 215));
            btnPause = MakeButton("⏸ Pausa", Color.FromArgb(255, 140, 0));
            btnStop = MakeButton("⏹ Stop", Color.FromArgb(108, 117, 125));
            btnDeleteSel = MakeButton("🗑️ Elimina Sel.", Color.FromArgb(220, 53, 69));
            btnSave = MakeButton("💾 Salva", Color.FromArgb(40, 167, 69));
            btnCancel = MakeButton("✖ Annulla", Color.FromArgb(108, 117, 125));

            btnPlay.Click += BtnPlay_Click;
            btnPause.Click += BtnPause_Click;
            btnStop.Click += BtnStop_Click;
            btnDeleteSel.Click += BtnDeleteSel_Click;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) => this.Close();

            int x = 10;
            foreach (var btn in new[] { btnPlay, btnPause, btnStop, btnDeleteSel, btnSave, btnCancel })
            {
                btn.Location = new Point(x, 8);
                x += btn.Width + 8;
                pnlButtons.Controls.Add(btn);
            }

            lblStatus = new Label
            {
                Text = "",
                AutoSize = true,
                ForeColor = Color.Silver,
                Location = new Point(x + 10, 15),
                Font = new Font("Segoe UI", 9f)
            };
            pnlButtons.Controls.Add(lblStatus);

            this.Controls.Add(pnlButtons);
        }

        private Button MakeButton(string text, Color back)
        {
            var btn = new Button
            {
                Text = text,
                Size = new Size(110, 32),
                BackColor = back,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f)
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void ApplyLanguage()
        {
            this.Text = LanguageManager.Get("AudioEditor.Title", "✏️ Audio Editor");
            btnPlay.Text = LanguageManager.Get("AudioEditor.BtnPlay", "▶ Play");
            btnPause.Text = LanguageManager.Get("AudioEditor.BtnPause", "⏸ Pausa");
            btnStop.Text = LanguageManager.Get("AudioEditor.BtnStop", "⏹ Stop");
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
            else
                LoadAudioSamples();
        }

        private void LoadAudioSamples()
        {
            try
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
                lblStatus.Text = $"{Path.GetFileName(_filePath)} | {_sampleRate} Hz | {_channels}ch | {(_samples.Length / _channels / _sampleRate):F1}s";
                picWaveform?.Invalidate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AudioEditor] Errore caricamento samples: {ex.Message}");
                lblStatus.Text = $"Errore: {ex.Message}";
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
                this.Text = LanguageManager.Get("AudioEditor.VideoPreviewTitle", "📺 Preview Video") + $" — {Path.GetFileName(_filePath)}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AudioEditor] Errore VLC init: {ex.Message}");
                lblStatus.Text = $"Errore video: {ex.Message}";
            }
        }

        // ═══════════════════════════════════════════════════════════
        // WAVEFORM PAINT & MOUSE
        // ═══════════════════════════════════════════════════════════
        private void PicWaveform_Paint(object sender, PaintEventArgs e)
        {
            if (_samples == null || _samples.Length == 0) return;

            var g = e.Graphics;
            int w = picWaveform.Width;
            int h = picWaveform.Height;
            int mid = h / 2;

            g.Clear(Color.FromArgb(30, 30, 30));

            // Center line
            g.DrawLine(Pens.DimGray, 0, mid, w, mid);

            // Selection highlight
            if (_selStart >= 0 && _selEnd > _selStart && _samples.Length > 0)
            {
                float x1 = (float)_selStart / _samples.Length * w;
                float x2 = (float)_selEnd / _samples.Length * w;
                using (var selBrush = new SolidBrush(Color.FromArgb(60, 0, 120, 215)))
                    g.FillRectangle(selBrush, x1, 0, x2 - x1, h);
                using (var selPen = new Pen(Color.FromArgb(0, 120, 215)))
                {
                    g.DrawLine(selPen, x1, 0, x1, h);
                    g.DrawLine(selPen, x2, 0, x2, h);
                }
            }

            // Waveform (downsampled)
            int step = Math.Max(1, _samples.Length / w);
            float gainLinear = (float)Math.Pow(10.0, _gainDb / 20.0);
            var wavePen = new Pen(Color.FromArgb(40, 167, 69));

            for (int px = 0; px < w; px++)
            {
                int idxStart = px * step;
                int idxEnd = Math.Min(idxStart + step, _samples.Length);
                float min = float.MaxValue, max = float.MinValue;
                for (int i = idxStart; i < idxEnd; i++)
                {
                    float s = _samples[i] * gainLinear;
                    if (s < min) min = s;
                    if (s > max) max = s;
                }
                min = Math.Max(-1f, min);
                max = Math.Min(1f, max);
                int yMax = (int)((1f - max) / 2f * h);
                int yMin = (int)((1f - min) / 2f * h);
                g.DrawLine(wavePen, px, yMax, px, Math.Max(yMin, yMax + 1));
            }
            wavePen.Dispose();
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
            if (_audioReader != null)
                _audioReader.Volume = (float)Math.Pow(10.0, _gainDb / 20.0);

            picWaveform?.Invalidate();
        }

        // ═══════════════════════════════════════════════════════════
        // PLAYBACK
        // ═══════════════════════════════════════════════════════════
        private void BtnPlay_Click(object sender, EventArgs e)
        {
            if (_isVideo)
            {
                _vlcPlayer?.Play();
                return;
            }

            try
            {
                StopAudio();
                _audioReader = new AudioFileReader(_filePath);
                _audioReader.Volume = (float)Math.Pow(10.0, _gainDb / 20.0);

                _waveOut = new WaveOutEvent();
                _waveOut.Init(_audioReader);
                _waveOut.PlaybackStopped += (s, ev) => { _isPlaying = false; };
                _waveOut.Play();
                _isPlaying = true;
                lblStatus.Text = "▶ " + Path.GetFileName(_filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AudioEditor] Errore play: {ex.Message}");
            }
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            if (_isVideo) { _vlcPlayer?.Pause(); return; }
            if (_waveOut != null && _isPlaying)
            {
                _waveOut.Pause();
                _isPlaying = false;
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (_isVideo) { _vlcPlayer?.Stop(); return; }
            StopAudio();
        }

        private void StopAudio()
        {
            try
            {
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

            // Rimuovi campioni selezionati
            var before = _samples.Take(_selStart).ToArray();
            var after = _samples.Skip(_selEnd).ToArray();
            _samples = before.Concat(after).ToArray();
            _selStart = -1;
            _selEnd = -1;

            TimeSpan newDuration = TimeSpan.FromSeconds((double)_samples.Length / _channels / _sampleRate);
            lblStatus.Text = $"✂️ Selezione eliminata | Nuova durata: {newDuration:mm\\:ss\\.ff}";
            picWaveform?.Invalidate();
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

                string tempPath = _filePath + ".tmp";
                float gainLinear = (float)Math.Pow(10.0, _gainDb / 20.0);

                // Applica gain ai samples prima di salvare (in-place quando possibile)
                float[] outputSamples = _samples;
                if (_gainDb != 0)
                {
                    outputSamples = new float[_samples.Length];
                    for (int i = 0; i < _samples.Length; i++)
                        outputSamples[i] = Math.Max(-1f, Math.Min(1f, _samples[i] * gainLinear));
                }

                // Scrivi il file tramite un file temporaneo e sostituisci l'originale
                WriteSamplesToWav(tempPath, outputSamples);
                File.Delete(_filePath);
                File.Move(tempPath, _filePath);

                // Aggiorna i samples con il gain applicato
                _samples = outputSamples;
                _gainDb = 0;
                trkGain.Value = 0;

                MessageBox.Show(
                    LanguageManager.Get("AudioEditor.SaveSuccess", "✅ File salvato con successo!"),
                    LanguageManager.Get("Common.Success", "Successo"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                lblStatus.Text = "💾 " + LanguageManager.Get("AudioEditor.SaveSuccess", "File salvato!");
                picWaveform?.Invalidate();
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
