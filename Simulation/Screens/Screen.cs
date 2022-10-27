using Game.ViewTools;
using SFML.Graphics;

namespace Game.Screens
{
    public class Screen
    {
        public Camera Camera { get; set; }

        public bool IsUpdate { get; set; }

        public bool IsUpdateCamera { get; set; }

        public bool IsDraw { get; set; }

        public RenderWindow window;

        public Screen(RenderWindow window, FloatRect configuration)
        {
            this.window = window;

            Camera = new Camera(configuration);

            IsUpdate = true;
            IsUpdateCamera = true;
            IsDraw = true;
        }

        public virtual void Update(float deltaT)
        {
        }

        public virtual void Draw(float deltaT)
        {
        }

        public virtual void InitializeScreen()
        {

        }

        public void SetInactive()
        {
            IsUpdate = false;
            IsDraw = false;
        }

        public void SetActive()
        {
            IsUpdate = true;
            IsDraw = true;
        }

        public void SetActiveState(bool isActive)
        {
            IsUpdate = isActive;
            IsDraw = isActive;
        }

        public void SetUpdateInactive()
        {
            IsUpdate = false;
        }

        public void SetDrawInactive()
        {
            IsDraw = false;
        }

        public void SetUpdateActive()
        {
            IsUpdate = true;
        }

        public void SetDrawActive()
        {
            IsDraw = true;
        }

        internal void Initialize()
        {
        }
    }
}
