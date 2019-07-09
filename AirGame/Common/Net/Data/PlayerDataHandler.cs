using GlLib.Common.Entities;

namespace GlLib.Common
{
    public static class PlayerDataHandler
    {
        /// <summary>
        /// Performs Validation of credantials by looking through whitelist(TODO or by calling to database)
        /// </summary>
        /// <param name="_entry"></param>
        /// <returns>Validity</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static bool CheckCredentials(PlayerDataEntry _entry)
        {
            throw new System.NotImplementedException();
        }
        
        /// <summary>
        /// Reads Data for this player from playerdata/%uuid%.dat if exist
        /// Otherwise calls CreateDataForPlayer() and returns result
        /// </summary>
        /// <param name="_entry"></param>
        /// <returns>Saved or Created PlayerData instance</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static PlayerData LoadPlayerData(PlayerDataEntry _entry)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Creates new PlayerData and saves it in separate file playerdata/%uuid%.dat
        /// </summary>
        /// <param name="_entry"></param>
        /// <returns>Created PlayerData instance</returns>
        /// <exception cref="NotImplementedException"></exception>
        private static PlayerData CreateDataForPlayer(PlayerDataEntry _entry)
        {
            throw new System.NotImplementedException();
        }
    }
}