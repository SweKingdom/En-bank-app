namespace BlazorApp4.Services
{
    /// <summary>
    /// Service responsible for managing the application's lock state
    /// Provides methods to lock and unlock the app using a PIN code,
    /// and notifies subscribed components whenever the lock state changes
    /// </summary>
    public class LockService
    {
        // Indicates whether the application is currently locked
        public bool IsLocked { get; private set; } = true;

        // Event triggered whenever the lock state changes
        public event Action? OnLockStateChanged;

        /// <summary>
        /// Attempts to unlock the application using the provided PIN code
        /// </summary>
        /// <param name="pin">The PIN code entered by the user</param>
        public void Unlock(string pin)
        {
            Console.WriteLine($"[LockService] Unlock attempt with pin: {pin}");
            if (pin == "1234")
            {
                IsLocked = false;
                OnLockStateChanged?.Invoke();
                Console.WriteLine("[LockService] App unlocked");
            }
            else
            {
                Console.WriteLine("[LockService] Wrong PIN");
            }
        }

        /// <summary>
        /// Locks the application and notifies all subscribers
        /// </summary>
        public void Lock()
        {
            IsLocked = true;
            Console.WriteLine("[LockService] App locked");
            OnLockStateChanged?.Invoke();
        }
    }
}
