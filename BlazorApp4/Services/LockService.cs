namespace BlazorApp4.Services
{
    public class LockService
    {
        public bool IsLocked { get; private set; } = true;
        public event Action? OnLockStateChanged;

        public void Unlock(string pin)
        {
            Console.WriteLine($"[LockService] Unlock attempt with pin: {pin}");
            if (pin == "1234")
            {
                IsLocked = false;
                OnLockStateChanged?.Invoke();
                Console.WriteLine("[LockService] ✅ App unlocked");
            }
            else
            {
                Console.WriteLine("[LockService] ❌ Wrong PIN");
            }
        }

        public void Lock()
        {
            IsLocked = true;
            Console.WriteLine("[LockService] 🔒 App locked");
            OnLockStateChanged?.Invoke();
        }
    }
}
