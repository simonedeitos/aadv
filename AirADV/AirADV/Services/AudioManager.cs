using System;
using NAudio.Wave;

namespace AirADV.Services
{
    /// <summary>
    /// Gestione audio player con NAudio
    /// </summary>
    public class AudioManager : IDisposable
    {
        private WaveOutEvent _waveOut;
        private AudioFileReader _audioFile;
        private System.Timers.Timer _positionTimer;

        public bool IsPlaying { get; private set; }
        public TimeSpan CurrentPosition => _audioFile?.CurrentTime ?? TimeSpan.Zero;
        public TimeSpan TotalDuration => _audioFile?.TotalTime ?? TimeSpan.Zero;

        private float _volume = 0.8f;
        public float Volume
        {
            get => _volume;
            set
            {
                _volume = Math.Max(0f, Math.Min(1f, value));
                if (_waveOut != null)
                    _waveOut.Volume = _volume;
            }
        }

        public event EventHandler PlaybackStarted;
        public event EventHandler PlaybackStopped;
        public event EventHandler<TimeSpan> PositionChanged;

        public AudioManager()
        {
            _positionTimer = new System.Timers.Timer(100);
            _positionTimer.Elapsed += (s, e) => PositionChanged?.Invoke(this, CurrentPosition);
        }

        public bool Load(string filePath)
        {
            try
            {
                Stop();

                if (!System.IO.File.Exists(filePath))
                {
                    Console.WriteLine($"[AudioManager] File non trovato: {filePath}");
                    return false;
                }

                _audioFile = new AudioFileReader(filePath);
                _waveOut = new WaveOutEvent();

                // Imposta device se configurato
                if (ConfigManager.OutputDeviceNumber >= 0 && ConfigManager.OutputDeviceNumber < WaveOut.DeviceCount)
                {
                    _waveOut.DeviceNumber = ConfigManager.OutputDeviceNumber;
                }

                _waveOut.Init(_audioFile);
                _waveOut.Volume = _volume;

                _waveOut.PlaybackStopped += (s, e) =>
                {
                    IsPlaying = false;
                    _positionTimer.Stop();
                    PlaybackStopped?.Invoke(this, EventArgs.Empty);
                };

                Console.WriteLine($"[AudioManager] File caricato: {System.IO.Path.GetFileName(filePath)} ({TotalDuration})");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AudioManager] Errore caricamento:  {ex.Message}");
                return false;
            }
        }

        public void Play()
        {
            if (_waveOut == null || _audioFile == null)
                return;

            try
            {
                _waveOut.Play();
                IsPlaying = true;
                _positionTimer.Start();
                PlaybackStarted?.Invoke(this, EventArgs.Empty);
                Console.WriteLine("[AudioManager] Playback started");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AudioManager] Errore play: {ex.Message}");
            }
        }

        public void Pause()
        {
            if (_waveOut == null)
                return;

            try
            {
                _waveOut.Pause();
                IsPlaying = false;
                _positionTimer.Stop();
                Console.WriteLine("[AudioManager] Playback paused");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AudioManager] Errore pause: {ex.Message}");
            }
        }

        public void Stop()
        {
            try
            {
                _positionTimer?.Stop();

                if (_waveOut != null)
                {
                    _waveOut.Stop();
                    _waveOut.Dispose();
                    _waveOut = null;
                }

                if (_audioFile != null)
                {
                    _audioFile.Dispose();
                    _audioFile = null;
                }

                IsPlaying = false;
                Console.WriteLine("[AudioManager] Playback stopped");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AudioManager] Errore stop:  {ex.Message}");
            }
        }

        public void Seek(TimeSpan position)
        {
            if (_audioFile == null)
                return;

            try
            {
                _audioFile.CurrentTime = position;
                Console.WriteLine($"[AudioManager] Seek to {position}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AudioManager] Errore seek: {ex.Message}");
            }
        }

        public static int GetDuration(string filePath)
        {
            try
            {
                using (var reader = new AudioFileReader(filePath))
                {
                    return (int)reader.TotalTime.TotalSeconds;
                }
            }
            catch
            {
                return 0;
            }
        }

        public void Dispose()
        {
            Stop();
            _positionTimer?.Dispose();
        }
    }
}