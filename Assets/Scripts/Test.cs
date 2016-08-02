/*============================================================================== 
 * Copyright (c) 2012-2015 Qualcomm Connected Experiences, Inc. All Rights Reserved. 
 * ==============================================================================*/
using UnityEngine;
using System.Collections;
using Vuforia;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class Test : MonoBehaviour
{
    #region PRIVATE_MEMBERS

    #endregion // PRIVATE_MEMBERS

    #region MONOBEHAVIOUR_METHODS
    void Start()
    {
		PlayVideoFullscreen ();
    }
    #endregion //MONOBEHAVIOUR_METHODS

    #region PRIVATE_METHODS
    private void PlayVideoFullscreen()
    {
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>();
        Collider[] colliderComponents = GetComponentsInChildren<Collider>();

        // Enable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = true;
        }

        // Enable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = true;
        }

        VideoPlaybackBehaviour video = GetComponentInChildren<VideoPlaybackBehaviour>();
		Debug.Log ("video.AutoPlay " + video.AutoPlay);
        if (video != null && video.AutoPlay)
        {
			//if (video.VideoPlayer.IsPlayableFullscreen())
			{
				//On Android, we use Unity's built in player, so Unity application pauses before going to fullscreen. 
				//So we have to handle the orientation from within Unity. 
				#if UNITY_ANDROID
				Screen.orientation = ScreenOrientation.LandscapeLeft;
				#endif
				// Pause the video if it is currently playing
				video.VideoPlayer.Pause();

				// Seek the video to the beginning();
				video.VideoPlayer.SeekTo(0.0f);

				// Display the busy icon
				video.ShowBusyIcon();

				// Play the video full screen
				StartCoroutine( PlayFullscreenVideoAtEndOfFrame(video) );
			}
        }
    }

	public static IEnumerator PlayFullscreenVideoAtEndOfFrame(VideoPlaybackBehaviour video)
	{
		Screen.orientation = ScreenOrientation.AutoRotation;
		Screen.autorotateToPortrait = true;
		Screen.autorotateToPortraitUpsideDown = true;
		Screen.autorotateToLandscapeLeft = true;
		Screen.autorotateToLandscapeRight = true;

		yield return new WaitForEndOfFrame ();

		// we wait a bit to allow the ScreenOrientation.AutoRotation to become effective
		yield return new WaitForSeconds (0.3f);

		video.VideoPlayer.Play(true, 0);
	}

    #endregion //PRIVATE_METHODS
}
