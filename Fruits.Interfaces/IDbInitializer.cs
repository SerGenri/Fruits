namespace Fruits.Interfaces;

/// <summary>
/// Создание БД
/// </summary>
public interface IDbInitializer
{
	/// <summary>
	/// Инициализация БД
	/// </summary>
	/// <param name="removeBefore">Удалить содержимое базы</param>
	/// <param name="initDb">Заполнять тестовыми данными</param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	Task InitializeAsync(bool removeBefore, bool initDb, CancellationToken cancel = default);
}