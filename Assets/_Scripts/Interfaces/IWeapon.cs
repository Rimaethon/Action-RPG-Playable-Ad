using _Scripts.Player;
using _Scripts.Scriptable_Objects;

namespace _Scripts.Interfaces
{
	public interface IWeapon
	{
		float GetRange();
		void TryGiveDamage();
		void InitializeWeapon(WeaponDataSO weaponData, EnemyDetector enemyDetector, PlayerAnimationManager playerAnimationManager);
	}
}
