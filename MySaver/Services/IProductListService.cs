using MySaver.Models;

namespace MySaver.Services;

public interface IProductListService
{
    void DeleteList(string listName);
    bool ListExists(string listName);
    IEnumerable<string> GetListNames();
    List<Product> OpenList(string listName);
    void RenameList(string oldName, string newName);
    void SaveList(string listName, IEnumerable<Product> list);
}