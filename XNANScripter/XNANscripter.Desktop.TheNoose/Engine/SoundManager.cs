using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Media;

namespace XNANScripter.Engine
{
    public class SoundManager
    {
        #region Dwave playing parameters

        private Dictionary<int, KeyValuePair<SoundEffect, SoundEffectInstance>  > dwaveList = new Dictionary<int, KeyValuePair<SoundEffect, SoundEffectInstance>>();
        private Dictionary<int, string> dwavePreloadedList = new Dictionary<int, string>();
        private float dwaveVolume = 1;

        #endregion Dwave playing parameters

        #region Wave playing parameters

        private float wavVolume = 1;
        private SoundEffectInstance wav;
        private SoundEffect wavEffect;
        #endregion Wave playing parameters

        #region Mp3 playing parameters

        private float mp3Volume = 1;
        private float mp3Fadein = 0;
        private float mp3Fadeout = 0;

        private double mp3Duration = 0;
        private double mp3CurrentTime = 0;
        private SoundEffectInstance mp3;
        private SoundEffect mp3Effect;
        #endregion Mp3 playing parameters

        #region Bgm playing parameters

        public bool bgmdownmode = false;

        private float bgmVolume = 1;
        private float bgmFadein = 0;
        private float bgmFadeout = 0;

        private double bgmDuration = 0;
        private double bgmCurrentTime = 0;
        private SoundEffectInstance bgm;
        private SoundEffect bgmEffect;
        #endregion Bgm playing parameters

        #region Loopbgm playing parameters

        private string loopbgmLoopedName;
        private bool loopbgmIsIntroPlayed = false;

        private double loopbgmDuration = 0;
        private double loopbgmCurrentTime = 0;
        private SoundEffectInstance loopbgm;
        private SoundEffect loopbgmEffect;
        #endregion Loopbgm playing parameters

        private Object Lock = new Object();

        internal void Update(TimeSpan delta)
        {
            lock (Lock)
            {

                //Mp3 fade in/out
                if (mp3 != null && mp3.State == SoundState.Playing)
                {
                    mp3CurrentTime += delta.TotalMilliseconds;

                    if (mp3Fadeout != 0 && mp3Duration >= mp3CurrentTime + mp3Fadeout)
                    {
                        mp3.Volume -= (float) (100/mp3Fadeout*delta.TotalMilliseconds)/100;
                    }
                    if (mp3Fadeout != 0 && mp3CurrentTime < mp3Fadein)
                    {
                        mp3.Volume += (float) (100/mp3Fadein*delta.TotalMilliseconds)/100;
                    }
                }
                //Bgm fade in/out
                if (bgm != null && bgm.State == SoundState.Playing)
                {
                    bgmCurrentTime += delta.TotalMilliseconds;

                    if (bgmFadeout != 0 && bgmDuration >= bgmCurrentTime + bgmFadeout)
                    {
                        bgm.Volume -= (float) (100/bgmFadeout*delta.TotalMilliseconds)/100;
                    }
                    if (bgmFadeout != 0 && bgmCurrentTime < bgmFadein)
                    {
                        bgm.Volume += (float) (100/bgmFadein*delta.TotalMilliseconds)/100;
                    }

                    //Bgm down mode
                    if (bgmdownmode)
                    {
                        throw new NotImplementedException();
                    }
                }
                //loopbgm fade in/out
                if (loopbgm != null)
                {
                    if (loopbgm.State == SoundState.Playing)
                    {
                        loopbgmCurrentTime += delta.TotalMilliseconds;

                        if (loopbgmDuration >= loopbgmCurrentTime + bgmFadeout)
                        {
                            loopbgm.Volume -= (float) (100/bgmFadeout*delta.TotalMilliseconds)/100;
                        }
                        if (loopbgmCurrentTime < bgmFadein)
                        {
                            loopbgm.Volume += (float) (100/bgmFadein*delta.TotalMilliseconds)/100;
                        }
                    }

                    if (!loopbgmIsIntroPlayed && loopbgm.State == SoundState.Stopped)
                    { 
                        loopbgm.Stop();
                        loopbgm.Dispose();
                        loopbgm = null;
                        loopbgmCurrentTime = 0;

                        using (System.IO.Stream stream = TitleContainer.OpenStream(@"Content\" + loopbgmLoopedName))
                        {
                            using (SoundEffect effect = SoundEffect.FromStream(stream))
                            {
                                loopbgmDuration = effect.Duration.TotalMilliseconds;
                                loopbgm = effect.CreateInstance();
                            }
                        }

                        loopbgm.Volume = bgmVolume;
                        loopbgm.IsLooped = true;
                        loopbgm.Play();

                        loopbgmIsIntroPlayed = true;
                    }
                }
            }
        }

        internal void Play(int type, int channel, string songName, bool repeat)
        {
            lock (Lock)
            {
                switch (type)
                {
                    case 1:
                        //check if midi
                        break;

                    case 2:
                        //wave
                        if (wav != null)
                        {
                            if (!wav.IsDisposed)
                            {
                                wav.Stop();
                                while (wav.State != SoundState.Stopped)
                                {
                                    Task.WaitAny(Task.Delay(100));
                                }
                                wav.Dispose();
                                wavEffect.Dispose();
                            }
                            wav = null;
                        }
                        
                        using (System.IO.Stream stream = TitleContainer.OpenStream(@"Content\" + songName))
                        {
                            wavEffect = SoundEffect.FromStream(stream);
                            wav = wavEffect.CreateInstance();
                        }
                        wav.Volume = wavVolume;
                        wav.IsLooped = repeat;
                        wav.Play();
                        break;

                    case 3:
                        //mp3
                        if (mp3 != null)
                        {
                            if (!mp3.IsDisposed)
                            {
                                mp3.Stop();
                                while (dwaveList[channel].Value.State != SoundState.Stopped)
                                {
                                    Task.WaitAny(Task.Delay(100));
                                }
                                mp3.Dispose();
                                mp3Effect.Dispose();
                            }
                            mp3 = null;
                        }
                        
                        using (System.IO.Stream stream = TitleContainer.OpenStream(@"Content\" + songName))
                        {
                            mp3Effect = SoundEffect.FromStream(stream);
                            mp3Duration = mp3Effect.Duration.TotalMilliseconds;
                            mp3 = mp3Effect.CreateInstance();
                        }
                        mp3CurrentTime = 0;
                        mp3.Volume = mp3Volume;
                        mp3.IsLooped = repeat;
                        mp3.Play();
                        break;

                    case 4:
                        //dwave
                        if (dwaveList.ContainsKey(channel))
                        {
                            if (!dwaveList[channel].Value.IsDisposed)
                            {
                                dwaveList[channel].Value.Stop();
                                while (dwaveList[channel].Value.State != SoundState.Stopped)
                                {
                                    Task.WaitAny(Task.Delay(100));
                                }
                                dwaveList[channel].Value.Dispose();
                                dwaveList[channel].Key.Dispose();
                            }
                            dwaveList.Remove(channel);
                        }
                        using (System.IO.Stream stream = TitleContainer.OpenStream(@"Content\" + songName))
                        {
                            var effect = SoundEffect.FromStream(stream);
                            dwaveList.Add(channel, new KeyValuePair<SoundEffect, SoundEffectInstance>(effect, effect.CreateInstance()));
                        }

                        dwaveList[channel].Value.Volume = dwaveVolume;
                        dwaveList[channel].Value.IsLooped = repeat;
                        dwaveList[channel].Value.Play();
                        break;

                    case 5:
                        //bgm
                        if (bgm != null)
                        {
                            if (!bgm.IsDisposed)
                            {
                                bgm.Stop();
                                while (bgm.State != SoundState.Stopped)
                                {
                                    Task.WaitAny(Task.Delay(100));
                                }
                                bgm.Dispose();
                                bgmEffect.Dispose();
                            }
                            bgm = null;
                        }


                        using (System.IO.Stream stream = TitleContainer.OpenStream(@"Content\" + songName))
                        {
                            bgmEffect = SoundEffect.FromStream(stream);
                            bgmDuration = bgmEffect.Duration.TotalMilliseconds;
                            bgm = bgmEffect.CreateInstance();
                        }

                        bgmCurrentTime = 0;
                        bgm.Volume = bgmVolume;
                        bgm.IsLooped = repeat;
                        bgm.Play();
                        
                        break;
                }
            }
        }

        internal void Stop(int type, int channel)
        {
            lock (Lock)
            {
                switch (type)
                {
                    case 0:
                        Stop(2, 0);
                        Stop(5, 0);
                        break;

                    case 1:
                        //check if midi

                        break;

                    case 2:
                        //wave
                        if (wav != null)
                        {
                            if (!wav.IsDisposed)
                            {
                                wav.Stop();
                                while (wav.State != SoundState.Stopped)
                                {
                                    Task.WaitAny(Task.Delay(100));
                                }
                                wav.Dispose();
                                wavEffect.Dispose();
                            }
                            wav = null;
                        }
                        break;

                    case 3:
                        //mp3
                        if (mp3 != null)
                        {
                            if (!mp3.IsDisposed)
                            {
                                mp3.Stop();
                                while (dwaveList[channel].Value.State != SoundState.Stopped)
                                {
                                    Task.WaitAny(Task.Delay(100));
                                }
                                mp3.Dispose();
                                mp3Effect.Dispose();
                            }
                            mp3 = null;
                        }
                        break;

                    case 4:
                        //dwave
                        if (dwaveList.ContainsKey(channel))
                        {
                            if (dwaveList[channel].Value!=null && !dwaveList[channel].Value.IsDisposed)
                            {
                                dwaveList[channel].Value.Stop();
                                while (dwaveList[channel].Value.State != SoundState.Stopped)
                                {
                                    Task.WaitAny(Task.Delay(200));
                                }
                                dwaveList[channel].Value.Dispose();
                                dwaveList[channel].Key.Dispose();
                            }
                            dwaveList.Remove(channel);
                        }
                        break;

                    case 5:
                        //bgm
                        if (bgm != null)
                        {
                            if (!bgm.IsDisposed)
                            {
                                bgm.Stop();
                                while (bgm.State != SoundState.Stopped)
                                {
                                    Task.WaitAny(Task.Delay(100));
                                }
                                bgm.Dispose();
                                bgmEffect.Dispose();
                            }
                            bgm = null;
                        }
                        break;

                    case 6:
                        //loopbgm TODO check later
                        if (loopbgm != null)
                        {
                            loopbgm.Stop();
                            while (loopbgm.State != SoundState.Stopped)
                            {
                                Task.WaitAny(Task.Delay(200));
                            }
                            loopbgm.Dispose();
                            loopbgm = null;
                            loopbgmCurrentTime = 0;
                            loopbgmIsIntroPlayed = false;

                        }
                        break;
                }
            }
        }

        #region Preloaded dwave

        internal void Load(int channel, string songName)
        {
            lock (Lock)
            {
                if (dwavePreloadedList.ContainsKey(channel))
                {
                    if (dwaveList.ContainsKey(channel))
                    {
                        dwaveList[channel].Value.Stop();
                        while (dwaveList[channel].Value.State != SoundState.Stopped)
                        {
                            Task.WaitAny(Task.Delay(200));
                        }
                        dwaveList[channel].Value.Dispose();
                        dwaveList[channel].Key.Dispose();
                        dwaveList.Remove(channel);
                    }

                    dwavePreloadedList.Remove(channel);
                }

                dwavePreloadedList.Add(channel, songName);
            }
        }

        internal void PlayLoaded(int channel, bool repeat)
        {
            lock (Lock)
            {
                if (dwavePreloadedList.ContainsKey(channel))
                {
                    if (dwaveList.ContainsKey(channel))
                    {
                        dwaveList[channel].Value.Stop();
                        while (dwaveList[channel].Value.State != SoundState.Stopped)
                        {
                            Task.WaitAny(Task.Delay(200));
                        }
                        dwaveList.Remove(channel);
                    }

                    //Надо учитывать огг файлы или решить это более глубокой конвертацией
                    using (
                        System.IO.Stream stream = TitleContainer.OpenStream(@"Content\" + dwavePreloadedList[channel]))
                    {
                        var effect = SoundEffect.FromStream(stream);
                        dwaveList.Add(channel, new KeyValuePair<SoundEffect, SoundEffectInstance>(effect,effect.CreateInstance()));
                    }

                    dwaveList[channel].Value.Volume = dwaveVolume;
                    dwaveList[channel].Value.IsLooped = repeat;
                    dwaveList[channel].Value.Play();
                }
                else
                {
                    throw new Exception("Указанный канал не задан.");
                }
            }
        }

        #endregion Preloaded dwave

        internal void SetChVolume(int channel, int volume)
        {
            lock (Lock)
            {
                if (dwaveList.ContainsKey(channel))
                {
                    dwaveList[channel].Value.Volume = volume;
                }
                else
                {
                    //Возможно надо учитывать и это
                    throw new Exception("Указанный канал не существует.");
                }
            }
        }

        internal void SetVoiceVolume(int volume)
        {
            lock (Lock)
            {
                wavVolume = (float) volume/100;
            }
        }

        internal void SetSeVolume(int volume)
        {
            lock (Lock)
            {
                //Так?
                dwaveVolume = (float) volume/100;
            }
        }

        #region Mp3 methods

        internal void SetMp3Volume(int volume)
        {
            lock (Lock)
            {
                mp3Volume = (float) volume/100;
            }
        }

        internal void SetMp3fadeout(int p)
        {
            lock (Lock)
            {
                mp3Fadeout = p;
            }
        }

        internal void SetMp3fadein(int p)
        {
            lock (Lock)
            {
                mp3Fadein = p;
            }
        }

        #endregion Mp3 methods

        #region Bgm methods

        internal void SetBgmVolume(int volume)
        {
            lock (Lock)
            {
                bgmVolume = (float) volume/100;
            }
        }

        internal void SetBgmfadeout(int p)
        {
            lock (Lock)
            {
                bgmFadeout = p;
            }
        }

        internal void SetBgmfadein(int p)
        {
            lock (Lock)
            {
                bgmFadein = p;
            }
        }

        #endregion Bgm methods

        #region Loopbgm methods

        internal void PlayLoopBgm(string intro, string looped)
        {
            lock (Lock)
            {
                loopbgmLoopedName = looped;

                if (loopbgm != null)
                {
                    loopbgm.Stop();
                }
                ;
                using (System.IO.Stream stream = TitleContainer.OpenStream(@"Content\" + intro))
                {
                    using (SoundEffect effect = SoundEffect.FromStream(stream))
                    {
                        loopbgmDuration = effect.Duration.TotalMilliseconds;
                        loopbgm = effect.CreateInstance();
                    }
                }
                loopbgmCurrentTime = 0;
                loopbgm.Volume = bgmVolume;
                loopbgm.IsLooped = false;
                loopbgm.Play();
            }
        }

        #endregion Loopbgm methods
    }
}