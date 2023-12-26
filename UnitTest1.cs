using LibraryProjectDll;
using LibraryProjectDll.Enums;
using LibraryProjectDll.Model;
using LibraryProjectDll.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestLibraryProject
{
    [TestClass]
    public class UnitTest1
    {
        LibraryService libraryService = new LibraryService();
        UserService userService = new UserService();
        //UserService Tests
        [TestMethod]
        public void TestSignUpAddNewUser()
        {
            Member member = new Member() { Username = "Nehorai123", Name = "Nehorai", Password = "1234" };
            int before = userService.GetAllMembers().Count;
            userService.SignUpUser(member);
            int after = userService.GetAllMembers().Count;
            Assert.IsTrue(before + 1 == after);
        }
        [TestMethod]
        public void TestSignUpWithOldUser()
        {
            Member member = new Member() { Username = "Nehorai1234", Name = "Nehorai", Password = "1234" };
            int before = userService.GetAllMembers().Count;
            try
            {
                userService.SignUpUser(member);
                userService.SignUpUser(member);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "This Account Already Exist");
            }
            int after = userService.GetAllMembers().Count;
            Assert.IsTrue(before + 1 == after);
        }
        [TestMethod]
        public void TestLoginSuccess()
        {
            User member = new Member() { Username = "oded123", Name = "oded", Password = "1234" };
            User user = userService.LogInUser(member.Username, member.Password);
            Assert.IsTrue(user != null);
        }
        [TestMethod]
        public void TestLoginFailed()
        {
            Member member = new Member() { Username = "1111", Password = "1111" };
            User user = userService.LogInUser(member.Username, member.Password);
            Assert.IsTrue(user == null);
        }
        [TestMethod]
        public void TestChangeDetails()
        {
            Member member = new Member() { Username = "Nehorai1234", Name = "Nehorai", Password = "1234" };            int before = userService.GetAllMembers().Count;
            try
            {
                userService.SignUpUser(member);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            finally
            {
            member.Username = "hello";
            userService.ChangeDetails(member);
            }
            User user = userService.LogInUser(member.Username, member.Password);
            Assert.IsTrue(user != null);
        }
        [TestMethod]
        public void TestUnActiveUser()
        {
            Member member = new Member() { Username = "stamuser", Name = "stam", Password = "1234" };
            try
            {
                userService.SignUpUser(member);
            }
            catch
            {

            }
            userService.UnActiveUser(member);
            Assert.IsTrue(!member.IsActive);
        }
        [TestMethod]
        public void TestActiveUser()
        {
            Member member = new Member() { Username = "Stamuser", Name = "stam", Password = "1234" };
            try
            {
                userService.SignUpUser(member);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            userService.UnActiveUser(member);
            userService.ActiveUser(member);
            Assert.IsTrue(member.IsActive);
        }
        [TestMethod]
        public void TestGetUserByIdSuccess()
        {
            Member member = new Member() { Username = "User123", Name = "user", Password = "1234" };
            try
            {
                userService.SignUpUser(member);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            User user = userService.GetUserById(member.Id);
            Assert.IsTrue(user!=null);
        }
        [TestMethod]
        public void TestGetUserByIdFaild()
        {
            Member member = new Member() { Username = "User", Name = "user", Password = "1234" };
            User user = userService.GetUserById(member.Id);
            Assert.IsTrue(user == null);
        }
        [TestMethod]
        public void TestGetAllMembers()
        {
            bool isnotmember = false;
            List<Member> members = userService.GetAllMembers();
            foreach(User member in members)
            {
                if(member is Labrarian)
                {
                    isnotmember = true;
                }
            }
            Assert.IsTrue(!isnotmember);
        }
        [TestMethod]
        public void TestGetAllLabrarians()
        {
            bool isnotlabrarian = false;
            List<Labrarian> members = userService.GetAllLabrarians();
            foreach (User member in members)
            {
                if (member is Member)
                {
                    isnotlabrarian = true;
                }
            }
            Assert.IsTrue(!isnotlabrarian);
        }
        //UserService Tests Ends

        //LibraryService Tests
        [TestMethod]
        public void TestAddLibraryItemSuccess()
        {
            LibraryItem item = new Book();
            int before = libraryService.GetAllBooks().Count + libraryService.GetAllJournals().Count;
            libraryService.AddLibraryItem(item);
            int after = libraryService.GetAllBooks().Count + libraryService.GetAllJournals().Count;
            Assert.IsTrue(before+1==after);
        }
        [TestMethod]
        public void TestAddLibraryItemFaild()
        {
            LibraryItem item = null;
            int before = libraryService.GetAllBooks().Count + libraryService.GetAllJournals().Count;
            libraryService.AddLibraryItem(item);
            int after = libraryService.GetAllBooks().Count + libraryService.GetAllJournals().Count;
            Assert.IsTrue(before == after);
        }
        [TestMethod]
        public void TestRemoveLibraryItemSuccess()
        {
            LibraryItem item = new Book();
            libraryService.AddLibraryItem(item);
            libraryService.RemoveLibraryItem(item);
            Assert.IsTrue(!item.IsAvaiable);
        }
        [TestMethod]
        public void TestUnRemoveLibraryItem()
        {
            LibraryItem item = new Book();
            libraryService.AddLibraryItem(item);
            libraryService.RemoveLibraryItem(item);
            libraryService.UnRemoveLibraryItem(item);
            Assert.IsTrue(item.IsAvaiable);
        }
        [TestMethod]
        public void TestChangeLibraryItemDetailsSuccess()
        {
            LibraryItem item = new Book() { Title="a tale with five baloons",ItemGenre=Genre.Kids  };
            libraryService.AddLibraryItem(item);
            item.Title = "Title";
            libraryService.ChangeLibraryItemDetails(item);
            Assert.IsTrue(libraryService.GetItemById(item.Id).Title==item.Title);
        }
        [TestMethod]
        public void TestRentBook()
        {
            Member member = new Member() { Username = "NewUser", Name = "user", Password = "1234" };
            try
            {
                userService.SignUpUser(member);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            User user = userService.GetUserById(member.Id);
            LibraryItem item = new Book() { Title = "a tale with five baloons", ItemGenre = Genre.Kids };
            libraryService.AddLibraryItem(item);
            libraryService.RentLibraryItem(item, user.Id);
            Assert.IsTrue(libraryService.GetItemById(item.Id).LibraryItemStatus==ItemStatus.Rented);
        }
        [TestMethod]
        public void TestReturnLibraryItem()
        {
            Member member = new Member() { Username = "newUser", Name = "user", Password = "1234" };
            try
            {
                userService.SignUpUser(member);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            User user = userService.GetUserById(member.Id);
            LibraryItem item = new Book() { Title = "a tale with five baloons", ItemGenre = Genre.Kids };
            libraryService.AddLibraryItem(item);
            libraryService.RentLibraryItem(item, user.Id);
            libraryService.ReturnLibraryItem(item);
            Assert.IsTrue(libraryService.GetItemById(item.Id).LibraryItemStatus == ItemStatus.Free);
        }
        [TestMethod]
        public void TestListOfRentedByUserId()
        {
            Member member = new Member() { Username = "RentUser", Name = "user", Password = "1234" };
            try
            {
                userService.SignUpUser(member);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            User user = userService.GetUserById(member.Id);
            LibraryItem item = new Book() { Title = "a tale with five baloons", ItemGenre = Genre.Kids };
            libraryService.AddLibraryItem(item);
            libraryService.RentLibraryItem(item, user.Id);
            List<LibraryItem> itemsrented=libraryService.ListOfRentedByUserId(member.Id);
            Assert.IsTrue(itemsrented.Count>0);
        }
        [TestMethod]
        public void ListOfAllTimeRentedByUserId()
        {
            Member member = new Member() { Username = "RentalltimeUser", Name = "user", Password = "1234" };
            try
            {
                userService.SignUpUser(member);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            LibraryItem item = new Book() { Title = "a tale with five baloons", ItemGenre = Genre.Kids };
            libraryService.AddLibraryItem(item);
            libraryService.RentLibraryItem(item, member.Id);
            libraryService.ReturnLibraryItem(item);
            List<LibraryItem> itemsrented = libraryService.ListOfAllTimeRentedByUserId(member.Id);
            Assert.IsTrue(itemsrented.Count > 0);
        }
        [TestMethod]
        public void ListOfAllRentedItems()
        {
            Member member = new Member() { Username = "RentedallUser", Name = "user", Password = "1234" };
            try
            {
                userService.SignUpUser(member);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            LibraryItem item = libraryService.GetAllFreeBooks().First();
            libraryService.AddLibraryItem(item);
            libraryService.RentLibraryItem(item, member.Id);
            List<LibraryItem> rented = libraryService.ListOfAllRentedItems();
            Assert.IsTrue(rented.Count>0);
        }
        [TestMethod]
        public void ListOfAllLatedItems()
        {
            Member member = new Member() { Username = "RentedallUser", Name = "user", Password = "1234" };
            try
            {
                userService.SignUpUser(member);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            User user = userService.GetUserById(member.Id);
            LibraryItem item = new Book() { Title = "Dragon Kid", ItemGenre = Genre.Kids ,DaysUntilReturn=-200 };
            libraryService.AddLibraryItem(item);
            libraryService.RentLibraryItem(item, user.Id);
            Assert.IsTrue(libraryService.ListOfAllLatedItems().Count > 0);
        }
        [TestMethod]
        public void GetAllFreeBooks()
        {
            Assert.IsTrue(libraryService.GetAllFreeBooks().Count > 0);
        }
        [TestMethod]
        public void GetAllFreeJournals()
        {
            Assert.IsTrue(libraryService.GetAllFreeJournals().Count > 0);
        }
        [TestMethod]
        public void GetAllBooks()
        {
            Assert.IsTrue(libraryService.GetAllBooks().Count > 0);
        }
        [TestMethod]
        public void GetAllJournals()
        {
            Assert.IsTrue(libraryService.GetAllJournals().Count > 0);
        }
    }
}