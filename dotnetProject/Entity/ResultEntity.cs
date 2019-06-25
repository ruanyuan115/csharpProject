using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetProject.Entity
{
    public class ResultEntity
    {
        public string message;
        public int? state;
        public Object data;

        public string getMessage() { return message; }
        public void setMessage(string message) { this.message = message; }
        public int? getState() { return state; }
        public void setState(int? state) { this.state = state; }
        public Object getData() { return data; }
        public void setData(Object data) { this.data = data; }

    }
}
