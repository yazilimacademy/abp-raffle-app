using System.Threading.Tasks;

namespace YazilimAcademy.ABPRaffleApp.Data;

public interface IABPRaffleAppDbSchemaMigrator
{
    Task MigrateAsync();
}
