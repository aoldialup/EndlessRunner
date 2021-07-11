using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WalmartEngine;

namespace WalmartMario
{
    class PowerupUIManager
    {
        private Dictionary<string, float> uiPowerups;

        private UIText powerupsDisplayText;

        private bool uiRequiresUpdate;

        private static PowerupUIManager _instance;

        public static PowerupUIManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PowerupUIManager();
                }

                return _instance;
            }
        }

        private PowerupUIManager()
        {
            uiPowerups = new Dictionary<string, float>();
            powerupsDisplayText = new UIText(string.Empty, new Font("Calibri", 14), new Point(350, 50), Color.White);
        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < uiPowerups.Count; i++)
            {
                KeyValuePair<string, float> status = uiPowerups.ElementAt(i);
                string tag = status.Key;
                float timeLeft = status.Value;

                if (timeLeft <= 0f)
                {
                    RemovePowerup(tag);
                }
                else
                {
                    uiPowerups[tag] -= Helper.ToSeconds(deltaTime);
                }
            }

            if (uiRequiresUpdate)
            {
                powerupsDisplayText.text = string.Empty;

                foreach (string p in uiPowerups.Keys)
                {
                    powerupsDisplayText.text += $"{p}\n";
                }

                uiRequiresUpdate = false;
            }
        }

        public bool PowerupExists(string powerupTag)
        {
            return uiPowerups.ContainsKey(powerupTag);
        }

        public void AddPowerup(Powerup p)
        {
            if (!PowerupExists(p.tag))
            {
                uiPowerups.Add(p.tag, p.timeUntilShouldRemove);
                uiRequiresUpdate = true;
            }
            else
            {
                uiPowerups[p.tag] = p.maxTimeOnUI;
            }
        }

        public void RemovePowerup(string powerupTag)
        {
            uiPowerups.Remove(powerupTag);
            uiRequiresUpdate = true;
        }

        public void Draw(Graphics gfx)
        {
            powerupsDisplayText.Draw(gfx);
        }
    }
}