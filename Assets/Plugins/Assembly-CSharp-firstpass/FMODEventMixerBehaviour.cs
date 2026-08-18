using UnityEngine.Playables;

public class FMODEventMixerBehaviour : PlayableBehaviour
{
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		int inputCount = playable.GetInputCount();
		float time = (float)playable.GetGraph().GetRootPlayable(0).GetTime();
		for (int i = 0; i < inputCount; i++)
		{
			((ScriptPlayable<FMODEventPlayableBehavior>)playable.GetInput(i)).GetBehaviour().UpdateBehaviour(time);
		}
	}
}
