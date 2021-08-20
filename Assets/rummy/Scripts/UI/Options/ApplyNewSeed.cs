using UnityEngine;
using UnityEngine.UI;
using FrolicRummy.Utility;

namespace FrolicRummy.UI.Options
{

    public class ApplyNewSeed : MonoBehaviour
    {
        public InputField NewSeedInput;
        public void ApplySeed()
        {
            int.TryParse(NewSeedInput.text, out int newSeed);
            Tb.I.GameMaster.Seed = newSeed;
        }
    }

}