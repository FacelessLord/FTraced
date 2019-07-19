using GlLib.Common.Items;

namespace GlLib.Common.Api.Inventory
{
    public interface IInventory
    {

        /// <summary>
        /// Maximal count of items enabled to place in
        /// </summary>
        /// <returns></returns>
        int GetMaxSize();

        /// <summary>
        /// Current amount of items in inventory
        /// </summary>
        /// <returns></returns>
        int GetCurrentSize();

        /// <summary>
        /// Currently selected slot
        /// </summary>
        int GetSelectedSlot();

        /// <summary>
        /// Selects inventory slot
        /// </summary>
        void SelectSlot(int _slotId);
        
        /// <summary>
        /// Places ItemStack to first available slot
        /// </summary>
        /// <param name="_itemStack"></param>
        void AddItemStack(ItemStack _itemStack);
        
        /// <summary>
        /// Places ItemStack to given slot
        /// </summary>
        /// <param name="_itemStack"></param>
        /// <param name="_slot"></param>
        void SetItemStack(ItemStack _itemStack, int _slot);
        
        /// <summary>
        /// Empties given slot
        /// </summary>
        /// <param name="_slot"></param>
        void RemoveItemStack(int _slot);
        
        /// <summary>
        /// Name of the inventory
        /// </summary>
        /// <returns></returns>
        string GetInventoryName();
        
        /// <summary>
        /// Gets stack from given slot
        /// </summary>
        /// <param name="_slot"></param>
        /// <returns></returns>
        ItemStack GetStackInSlot(int _slot);

        /// <summary>
        /// Whether ItemStack is enabled to put in given slot
        /// </summary>
        /// <param name="_item">Item to place</param>
        /// <param name="_slot">Slot to place item to</param>
        /// <returns></returns>
        bool IsItemValidForSlot(ItemStack _item, int _slot);
    }
}