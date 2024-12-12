using System;

class AVLTreeNode
{
    public int Key;
    public AVLTreeNode Left, Right;
    public int Height;

    public AVLTreeNode(int key)
    {
        Key = key;
        Height = 1;  // Yeni düğüm eklenince başlangıçta yükseklik 1.
    }
}

class AVLTree
{
    public AVLTreeNode Root;

    // Yükseklik hesaplama
    private int Height(AVLTreeNode node)
    {
        return node == null ? 0 : node.Height;
    }

    // Denge faktörünü hesapla
    private int GetBalanceFactor(AVLTreeNode node)
    {
        return node == null ? 0 : Height(node.Left) - Height(node.Right);
    }

    // Sol rotasyon
    private AVLTreeNode LeftRotate(AVLTreeNode z)
    {
        AVLTreeNode y = z.Right;
        z.Right = y.Left;
        y.Left = z;
        z.Height = Math.Max(Height(z.Left), Height(z.Right)) + 1;
        y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;
        return y;
    }

    // Sağ rotasyon
    private AVLTreeNode RightRotate(AVLTreeNode y)
    {
        AVLTreeNode x = y.Left;
        y.Left = x.Right;
        x.Right = y;
        y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;
        x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;
        return x;
    }

    // Düğüm ekleme
    public AVLTreeNode Insert(AVLTreeNode node, int key)
    {
        if (node == null)
            return new AVLTreeNode(key);

        if (key < node.Key)
            node.Left = Insert(node.Left, key);
        else if (key > node.Key)
            node.Right = Insert(node.Right, key);
        else
            return node; // Aynı değeri eklememek için

        node.Height = Math.Max(Height(node.Left), Height(node.Right)) + 1;

        int balance = GetBalanceFactor(node);

        // Dengeyi sağlamak için dönüşümler
        if (balance > 1 && key < node.Left.Key)  // Sol-sol
            return RightRotate(node);
        if (balance < -1 && key > node.Right.Key)  // Sağ-sağ
            return LeftRotate(node);
        if (balance > 1 && key > node.Left.Key)  // Sol-sağ
        {
            node.Left = LeftRotate(node.Left);
            return RightRotate(node);
        }
        if (balance < -1 && key < node.Right.Key)  // Sağ-sol
        {
            node.Right = RightRotate(node.Right);
            return LeftRotate(node);
        }

        return node;
    }

    // Ağaç yapısını görsel olarak yazdırma
    public void PrintTree(AVLTreeNode node, string indent = "", bool last = true)
    {
        if (node != null)
        {
            Console.Write(indent);
            if (last)
            {
                Console.Write("└── ");
                indent += "    ";
            }
            else
            {
                Console.Write("├── ");
                indent += "|   ";
            }

            Console.WriteLine(node.Key);

            PrintTree(node.Left, indent, false);
            PrintTree(node.Right, indent, true);
        }
    }

    // Arama işlemi
    public bool Search(AVLTreeNode node, int key)
    {
        if (node == null)
            return false;

        if (node.Key == key)
            return true;

        if (key < node.Key)
            return Search(node.Left, key);
        else
            return Search(node.Right, key);
    }

    // Düğüm silme
    public AVLTreeNode Delete(AVLTreeNode node, int key)
    {
        if (node == null) return node;

        if (key < node.Key)
            node.Left = Delete(node.Left, key);
        else if (key > node.Key)
            node.Right = Delete(node.Right, key);
        else
        {
            // Düğüm bulundu
            if (node.Left == null || node.Right == null)
            {
                AVLTreeNode temp = node.Left ?? node.Right;
                node = temp;
            }
            else
            {
                AVLTreeNode temp = MinValueNode(node.Right);
                node.Key = temp.Key;
                node.Right = Delete(node.Right, temp.Key);
            }
        }

        if (node == null)
            return node;

        node.Height = Math.Max(Height(node.Left), Height(node.Right)) + 1;

        int balance = GetBalanceFactor(node);

        if (balance > 1 && GetBalanceFactor(node.Left) >= 0)  // Sol-sol
            return RightRotate(node);
        if (balance < -1 && GetBalanceFactor(node.Right) <= 0)  // Sağ-sağ
            return LeftRotate(node);
        if (balance > 1 && GetBalanceFactor(node.Left) < 0)  // Sol-sağ
        {
            node.Left = LeftRotate(node.Left);
            return RightRotate(node);
        }
        if (balance < -1 && GetBalanceFactor(node.Right) > 0)  // Sağ-sol
        {
            node.Right = RightRotate(node.Right);
            return LeftRotate(node);
        }

        return node;
    }

    // Minimum değerli düğüm
    private AVLTreeNode MinValueNode(AVLTreeNode node)
    {
        AVLTreeNode current = node;
        while (current.Left != null)
            current = current.Left;
        return current;
    }
}

class Program
{
    static void Main()
    {
        AVLTree tree = new AVLTree();
        int choice, key;

        while (true)
        {
            // Menü
            Console.WriteLine("\nAVL Ağacı Menüsü");
            Console.WriteLine("1. Düğüm Ekle");
            Console.WriteLine("2. Düğüm Sil");
            Console.WriteLine("3. Düğüm Ara");
            Console.WriteLine("4. Ağacı Yazdır");
            Console.WriteLine("5. Çıkış");
            Console.Write("Bir seçenek girin: ");
            choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    // Düğüm Ekle
                    Console.Write("Eklenecek düğümün değerini girin: ");
                    key = int.Parse(Console.ReadLine());
                    tree.Root = tree.Insert(tree.Root, key);
                    Console.WriteLine($"{key} ağaçta başarıyla eklendi.");
                    break;

                case 2:
                    // Düğüm Sil
                    Console.Write("Silinecek düğümün değerini girin: ");
                    key = int.Parse(Console.ReadLine());
                    tree.Root = tree.Delete(tree.Root, key);
                    Console.WriteLine($"{key} ağaçtan başarıyla silindi.");
                    break;

                case 3:
                    // Düğüm Ara
                    Console.Write("Aranacak düğümün değerini girin: ");
                    key = int.Parse(Console.ReadLine());
                    if (tree.Search(tree.Root, key))
                        Console.WriteLine($"{key} ağaçta bulundu.");
                    else
                        Console.WriteLine($"{key} ağaçta bulunamadı.");
                    break;

                case 4:
                    // Ağacı Yazdır
                    Console.WriteLine("Ağacın yapısı:");
                    tree.PrintTree(tree.Root);
                    break;

                case 5:
                    // Çıkış
                    Console.WriteLine("Programdan çıkılıyor...");
                    return;

                default:
                    Console.WriteLine("Geçersiz bir seçenek girdiniz. Lütfen tekrar deneyin.");
                    break;
            }
        }
    }
}
