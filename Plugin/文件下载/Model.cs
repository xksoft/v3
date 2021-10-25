using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 文件下载
{
  public  class Model
    {
      public class download
      {

          public Url url = new Url();
          public string ext = "";
          public string filename = "";
      }
      public class Url
      {
          public string full_url = "";
          public string old_url = "";
      }
    }
}
