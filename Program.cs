using System;
using System.IO;

class BankAccount
{
    public string cardNumber;
    public string cardExpiration;
    public string cardCVC;
    public string ownerName;
    public double balance;

    public BankAccount()
    {
        if (File.Exists("account.txt"))
        {
            LoadAccountData();
        }
        else
        {
            InitializeAccount();
        }
    }

    public void ShowBalance()
    {
        Console.WriteLine($"Баланс рахунку: {balance}");
    }

    public void Deposit(double amount)
    {
        if (amount > 0)
        {
            balance += amount;
            SaveAccountData();
        }
    }

    public void Withdraw(double amount)
    {
        if (amount > 0 && amount <= balance)
        {
            balance -= amount;
            SaveAccountData();
        }
    }

    public void ShowInfo()
    {
        Console.WriteLine($"ПІБ власника: {ownerName}");
        Console.WriteLine($"Номер картки: {cardNumber}");
        Console.WriteLine($"термін придатності: {cardExpiration}");
        Console.WriteLine($"CVC код: {cardCVC}");
    }

    public void InitializeAccount()
    {
        Random random = new Random();
        cardNumber = GenerateCardNumber(random);
        cardExpiration = GenerateCardExpiration(random);
        cardCVC = GenerateCardCVC(random);

        Console.Write("Введіть ПІБ власника рахунку: ");
        ownerName = Console.ReadLine();
    }

    public static string GenerateCardNumber(Random random)
    {
        string cardNumber = "";
        for (int i = 0; i < 16; i++)
        {
            cardNumber += random.Next(10).ToString();
        }
        return cardNumber;
    }

    public static string GenerateCardExpiration(Random random)
    {
        int year = DateTime.Now.Year + random.Next(1, 6);
        int month = random.Next(1, 13);
        return $"{month:D2}/{year % 100:D2}";
    }

    private static string GenerateCardCVC(Random random)
    {
        return random.Next(100, 1000).ToString();
    }

    public void SaveAccountData()
    {
        using (StreamWriter writer = new StreamWriter("account.txt"))
        {
            writer.WriteLine(cardNumber);
            writer.WriteLine(cardExpiration);
            writer.WriteLine(cardCVC);
            writer.WriteLine(ownerName);
            writer.WriteLine(balance);
        }
    }

    public void LoadAccountData()
    {
        using (StreamReader reader = new StreamReader("account.txt"))
        {
            cardNumber = reader.ReadLine();
            cardExpiration = reader.ReadLine();
            cardCVC = reader.ReadLine();
            ownerName = reader.ReadLine();
            if (double.TryParse(reader.ReadLine(), out double savedBalance))
            {
                balance = savedBalance;
            }
        }
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        BankAccount account = new BankAccount();

        while (true)
        {
            Console.Clear();

            Console.WriteLine("\nМеню управління:");
            account.ShowBalance();
            Console.WriteLine("1. Поповнити рахунок");
            Console.WriteLine("2. Зняти гроші");
            Console.WriteLine("3. Показати інформацію про банківський рахунок");
            Console.WriteLine("4. Вихід");

            Console.Write("Выберіть опцію: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DepositMoney(account);
                    break;

                case "2":
                    WithdrawMoney(account);
                    break;

                case "3":
                    DisplayAccountInfo(account);
                    break;

                case "4":
                    account.SaveAccountData();
                    Console.WriteLine("Завершення роботи.");
                    return;

                default:
                    Console.WriteLine("Некоректний вибір. Спробуйте ще раз.");
                    break;
            }
        }
    }

    static void DepositMoney(BankAccount account)
    {
        Console.Clear();
        double depositAmount;
        while (true)
        {
            Console.Write("Введіть суму для поповнення: ");
            if (double.TryParse(Console.ReadLine(), out depositAmount) && depositAmount > 0)
            {
                account.Deposit(depositAmount);
                break;
            }
            else
            {
                Console.WriteLine("Сума має бути позитивною і числовою.");
            }
        }
    }

    static void WithdrawMoney(BankAccount account)
    {
        Console.Clear();
        double withdrawAmount;
        while (true)
        {
            Console.Write("Введіть суму для зняття: ");
            if (double.TryParse(Console.ReadLine(), out withdrawAmount) && withdrawAmount > 0)
            {
                if (withdrawAmount <= account.balance)
                {
                    account.Withdraw(withdrawAmount);
                    break;
                }
                else
                {
                    Console.WriteLine("Недостатньо грошей на рахунку.");
                }
            }
            else
            {
                Console.WriteLine("Сума має бути позитивною і числовою.");
            }
        }
    }

    static void DisplayAccountInfo(BankAccount account)
    {
        Console.Clear();
        account.ShowInfo();
        Console.WriteLine("Натисніть Enter для повернення в меню.");
        Console.ReadLine();
    }
}
