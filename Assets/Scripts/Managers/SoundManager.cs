using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
   public static SoundManager instance;

   [System.Serializable]
   public enum Sound
   {
      PlayerSwing = 0,
      PauseClick = 1,
      PlayerGetHit = 2,
      BoxHit = 3,
      SwingHitEnemy = 4,
      BoxTouchEnemy = 5,
      EnemyDeath = 6,
      // PlayerDeath = 7,
      BalloonPop = 8,
      Explosion = 9,
   }

   [System.Serializable]
   public class SoundAudioClip
   {
      public Sound sound;
      public AudioClip audioClip;
      public int quantity = 5;
   }

   public List<SoundAudioClip> sounds;

   private Dictionary<Sound, Queue<AudioSource>> dict;
   private Dictionary<Sound, bool> canPlayDict;

   private float sfxVolume = 0.20f;

   private void Awake()
   {
      if (instance == null)
      {
         instance = this;
         InitializeDictionary();
         DontDestroyOnLoad(gameObject);
      }
      else
      {
         Destroy(gameObject);
      }
   }

   // Each sound gets one object with multiple AudioSource components
   private void InitializeDictionary()
   {
      dict = new Dictionary<Sound, Queue<AudioSource>>();
      canPlayDict = new Dictionary<Sound, bool>();

      // Creates a dictionary for each audio source containing each different sound that can be played
      foreach (SoundAudioClip clip in sounds)
      {
         Queue<AudioSource> audioPool = new Queue<AudioSource>();
         GameObject soundObject = new GameObject(clip.sound.ToString());
         // All audio sources are persistent through scenes
         DontDestroyOnLoad(soundObject);

         for (int i = 0; i < clip.quantity; i++)
         {
            AudioSource source = soundObject.AddComponent<AudioSource>();
            source.tag = "SoundEffect";
            source.clip = clip.audioClip;
            source.volume = sfxVolume;
            source.playOnAwake = false;
            audioPool.Enqueue(source);
         }

         dict.Add(clip.sound, audioPool);

         canPlayDict.Add(clip.sound, true);
      }
   }

   public void PlaySound(Sound sound, bool randomizePitch = true, float volumeDelta = 0)
   {
      if (!canPlayDict[sound])
         return;

      AudioSource toPlay = dict[sound].Dequeue();

      if (randomizePitch)
         toPlay.pitch = Random.Range(0.95f, 1.05f);
      else
         toPlay.pitch = 1;
      toPlay.volume = sfxVolume + volumeDelta;

      toPlay.Play();
      dict[sound].Enqueue(toPlay);
      StartCoroutine(CannotPlaySound(sound));
   }

   public void PlaySoundPitch(Sound sound, float pitch, float volumeDelta = 0)
   {
      if (!canPlayDict[sound])
         return;

      AudioSource toPlay = dict[sound].Dequeue();
      toPlay.pitch = pitch;
      toPlay.volume = sfxVolume + volumeDelta;

      toPlay.Play();
      dict[sound].Enqueue(toPlay);
      StartCoroutine(CannotPlaySound(sound));
   }

   private IEnumerator CannotPlaySound(Sound sound)
   {
      canPlayDict[sound] = false;

      yield return null;
      yield return null;

      canPlayDict[sound] = true;
   }
}
