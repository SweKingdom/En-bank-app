namespace BlazorApp4.Services
{
    /// <summary>
    /// Tracks whether the user is currently logged in and
    /// provides an event-based mechanism
    /// to notify UI components when the state changes
    /// </summary>
    public class AppState
    {
        // Backing field indicating the user's login status
        public bool IsLoggedIn { get; private set; } = false;

        /// <summary>
        /// Event triggered whenever the application state changes.
        /// Components can subscribe to this to refresh their UI
        /// </summary>
        public event Action? OnChange;

        /// <summary>
        /// Sets the login status and triggers a state change notification
        /// </summary>
        /// <param name="value">New login status</param>
        public void SetLoggedIn(bool value)
        {
            IsLoggedIn = value;
            NotifyStateChanged();
        }
        private void NotifyStateChanged() => OnChange?.Invoke();    }
}
