using TMPro;
using UnityEngine;
using UnityEngine.UI;

// �ϳɴ��� UI ����ʾ���߼���
public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;                              // �ϳɽ��������
    [SerializeField] private TextMeshProUGUI itemDescription;                       // �ϳɽ��������
    [SerializeField] private Image itemIcon;                                        // �ϳɽ����ͼ��
    [SerializeField] private Image[] materialImage;                                 // ���ϲ�λ��ͼƬ + ������
    [SerializeField] private Button craftButton;                                    // �ϳɰ�ť

    /// <summary>
    /// ���úϳɴ��ڵ���ʾ����
    /// </summary>
    /// <param name="_data">Ҫ�ϳɵ�װ������</param>
    public void SetupCraftWindow(ItemData_Equipment _data)
    {
        craftButton.onClick.RemoveAllListeners();                                   // ��հ�ť��֮ǰ�󶨵ĵ���¼��������ظ���

        // �Ȱ����в��ϲ���գ����أ�
        for (int i = 0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear;                                   // ����ͼƬ
            // ��������
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        // ������ǰ��Ʒ�ĺϳɲ��ϣ���� UI
        for (int i = 0; i < _data.craftingMaterials.Count; i++)
        {
            // ���ò���ͼ��
            materialImage[i].sprite = _data.craftingMaterials[i].data.icon;
            materialImage[i].color = Color.white;

            // ���ò�����������
            TextMeshProUGUI materialSlotText = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();
            materialSlotText.text = _data.craftingMaterials[i].stackSize.ToString();
            materialSlotText.color = Color.white;
        }

        // ���½����Ʒ����Ϣ
        itemIcon.sprite = _data.icon;                                               // ��ʾ�����Ʒ��ͼ��
        itemName.text = _data.itemName;                                             // ��ʾ����
        itemDescription.text = _data.GetDescription();                              // ��ʾ����

        // ���ϳɰ�ť���߼������ʱ����Ƿ��ܺϳ�
        // ������ Inventory.instance.CanCraft ���ж��Ƿ�����㹻��ִ�кϳɣ�
        craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(_data, _data.craftingMaterials));
    }
}
