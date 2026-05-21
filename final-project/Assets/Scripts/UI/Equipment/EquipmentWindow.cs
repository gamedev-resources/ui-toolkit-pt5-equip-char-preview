using UnityEngine;
using UnityEngine.UIElements;

public class EquipmentWindow : MonoBehaviour
{
    private static readonly string[] SLOT_NAMES =
    {
        "slot-head", "slot-weapon", "slot-shield",
        "slot-potion", "slot-accessory-1", "slot-utility"
    };

    [Header("Templates")]
    [SerializeField] private VisualTreeAsset _equipmentWindowTemplate;
    [SerializeField] private VisualTreeAsset _itemSlotTemplate;

    [Header("Layout")]
    [SerializeField] private float _windowWidth = 520f;

    public void BuildEquipment(GameWindow window)
    {
        if (_equipmentWindowTemplate == null || _itemSlotTemplate == null)
        {
            Debug.LogError("Equipment Window Template or Item Slot Template is not assigned.");
            return;
        }

        var width = _windowWidth > 0 ? _windowWidth : 520f;

        window.Root.style.width = width;
        window.Root.style.height = StyleKeyword.Auto;

        var content = _equipmentWindowTemplate.Instantiate().ExtractRoot("equipment-content");

        foreach (var slotName in SLOT_NAMES)
        {
            var placeholder = content.Q<VisualElement>(slotName);
            if (placeholder == null) continue;

            var parent = placeholder.parent;
            int index = parent.IndexOf(placeholder);
            parent.Remove(placeholder);
            parent.Insert(index, new InventorySlot(_itemSlotTemplate));
        }

        window.ContentArea.Add(content);
    }
}

