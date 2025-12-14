# 📦 Military Inventory Manager

> 🛠️ Stock Management System created in C# (Windows Forms) to optimize the control of items designated for the kitchen/supply depot of a military organization.

---

## ✨ Key Features

The **Inventory Manager** is designed to be a lightweight and robust solution, focusing on three main pillars:

* **Product Control:** Quick registration, editing, and consultation of items via barcode (`Barcode`) or name.
* **Movement Management:** Simplified process for withdrawing (selling/consuming) items from stock.
* **Traceability and Documentation:** Automatic generation of withdrawal (sales/consumption) reports in PDF format.

---

## 💻 Technologies Used

| Technology | Description |
| :--- | :--- |
| **C\#** | Primary programming language (Windows Forms). |
| **.NET Framework** | Runtime environment and libraries. |
| **Microsoft Access (`.accdb`)** | Local database for product storage. |
| **OLE DB** | Data provider for connection to Access. |
| **PDF Generation (Custom)** | Class responsible for creating movement reports. |

---

## ⚙️ Code Structure

The project is organized into modular classes that separate the presentation logic (Forms) from the business logic and data persistence.

### 1. `Product.cs`
Defines the main entity, representing an inventory item:
* `Barcode`, `Name`, `UF` (Unit of Measure).
* `Value` (Price), `Validate` (Expiration Date).
* `minStock` (Minimum Stock), `Amount` (Current Quantity).

### 2. `RegisterNewProduct.cs` (Business Logic)
Responsible for all database interaction for inclusion and consultation:
* `AddNewProduct()`: Inserts a new product into the database.
* `UpdateProduct()`: Updates the current quantity (used for stock replenishment).
* `ChangeID()`: Searches for a product by `CodBarras` (Barcode) to populate the registration form fields, facilitating editing or stock updates.

### 3. Forms (UI)

* **`RegisterForm.cs`:**
    * Interface for registering new products and updating the minimum stock/details of existing items.
    * Handles text and value change events to synchronize with the `RegisterNewProduct` class.
* **`SellForm.cs`:**
    * Interface dedicated to **stock withdrawal** (simulating sale/consumption).
    * Allows searching for the product by ID or Name.
    * Adds products to a transaction list (`productsToRemove`).
    * `RemoveProductToDB()`: Executes the *decrement* of the quantity in the database for all items in the list and then calls the PDF generator.

### 4. `PDFGenerator.cs` (Inferred)
This class is responsible for taking the list of withdrawn products (`productsToRemove`), along with information such as the Storekeeper (`Paioleiro`) and the Receiver (`Recebedor`), and generating an output report in PDF format for auditing and military record-keeping purposes.

---

## 🖼️ Interface Overview

The system uses standard Windows Forms for fast and functional interaction.



### Withdrawal Flow (Stock Output)

1.  The user enters the **Barcode** or **Name** in the `SellForm`.
2.  The system loads the item details (Name, Price, Current Qty).
3.  The user enters the **Quantity** to be withdrawn.
4.  The item is added to the transaction list (ListView).
5.  After adding all items, the user fills in the **Storekeeper** (who released it) and the **Receiver** (who accepted it) and clicks **Sell/Withdraw**.
6.  The system updates the stock in the database and generates the withdrawal report PDF.



---

## ⚠️ Database Configuration

The database (MS Access) is expected in the following path, which uses the user's local application data folder:

```csharp
// Base Path: C:\Users\<Username>\AppData\Local\GerenciadorDeEstoque\EstoquePaiol.accdb
static string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
static string pastaBanco = Path.Combine(localAppData, "GerenciadorDeEstoque");
static string dbPath = Path.Combine(pastaBanco, "EstoquePaiol.accdb");