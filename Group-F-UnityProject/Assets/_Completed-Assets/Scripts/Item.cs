[System.Serializable]
public class Item
{
    public string itemName; // アイテムの名前
    public int quantity;    // アイテムの個数

    public Item(string itemName, int quantity)
    {
        this.itemName = itemName;
        this.quantity = quantity;
    }

    // アイテム情報を文字列で返す（デバッグ用）
    public override string ToString()
    {
        return $"{itemName}: {quantity}";
    }
}
