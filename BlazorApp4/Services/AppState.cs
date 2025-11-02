namespace BlazorApp4.Services
{
    public class AppState
    {
        public bool IsLoggedIn { get; private set; } = false;
        public event Action? OnChange;

        public void SetLoggedIn(bool value)
        {
            IsLoggedIn = value;
            NotifyStateChanged();
        }
        private void NotifyStateChanged() => OnChange?.Invoke();    }
}
