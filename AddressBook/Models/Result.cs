using System;
namespace AddressBook.Models
{

    public class Result 
    {
        public int Code
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }
    }

    public class Result<T> : Result
    {
        public T Content
        {
            get;
            set;
        }
    }
}
