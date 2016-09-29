ToolGood.ReadyGo
===


��ӭʹ��`ToolGood.ReadyGo`��`ToolGood.ReadyGo` ��һ����������ORM��
���򵥣���Ϊ������߶��꾭�飬����ݣ�����PetaPoco���ģ�
�������Ӵ���Ŀɶ��ԡ�
���ṩ��VS�����ʹCoding�����㡣

### ��������

```` csharp
public class User
{
	public int Id {get;set;}
	public string UserName {get;set;}
	public string Password {get;set;}
	public string NickName {get;set;}
}

using ToolGood.ReadyGo;

public User FindUser(int userId,string userName,string nickName)
{
    var helper = SqlHelperFactory.OpenMysql("127.0.0.1", "web", "root", "123456");
    return helper.CreateWhere<User>()
        .IfTrue(userId > 0).Where((u) => u.Id == userId)
        .IfSet(userName).Where((u) => u.UserName == userName)
        .IfSet(nickName).Where((u) => u.NickName == nickName)
        .SingleOrDefault();
}
`````


### ���ܼ�飺

* �������
  * ֧�ֱ�����ɾ��
  * ֧����������
  * ֧��Ψһ����������
  * ����Attribute
* ֧��Object���ٲ��롢�޸ġ�ɾ��
  * ֧���޸�ǰ�¼���
* ֧��ԭ��SQL����
  * ֧��SQL��
  * ֧�ַ�ҳ��ѯ��
  * ��ѯ��ִ��OnLoaded ����
* ֧�ֶ�̬SELECT
  * ֧�ַ���dynamic��
  * �﷨����LINQ
  * ֧�ֶ�̬���SELECT��
  * ֧�ֱ������Ρ�
* ֧�ֶ�̬UPDATE
  * �﷨����LINQ��
  * ����AOP˼·�����ƻ�ԭ���ࡣ
* ֧�ִ洢���̡�
* ֧�ֻ��档
* ֧�ִ����Ķ�д���롣
* ֧��SQLִ�м�ء�
* ��Sql Parser��
* ��VS Coding�����


#### 1�����ݱ�������ɾ��

##### 1.1���򵥵����ݱ����
Ŀǰ֧�֡�������������ݿ���Sql Server��MySql��SQLite��

```` csharp
using ToolGood.ReadyGo.Attributes;

public class User
{
    public int Id {get;set;}
    public int UserType {get;set;}
    public string UserName {get;set;}
    public string Password {get;set;}
    public string NickName {get;set;}
}


using ToolGood.ReadyGo;

var helper = SqlHelperFactory.OpenSqliteFile(dbFile);
var table = helper.TableHelper;
table.CreateTable<User>();
table.CreateIndex<User>();
table.CreateUnique<User>();
table.DeleteTable<User>();
````

##### 1.2��ToolGood.ReadyGo.Attributes ����
��`ToolGood.ReadyGo.Attributes`�����ռ����ṩ���¼���Attribute
* TableAttribute   ����Class�����������schema������������TAG����
* PrimaryKeyAttribute ����Class���������������Զ����ӡ�Sequence����
* IndexAttribute ����Class������������
* UniqueAttribute ����Class������Ψһ������
* ColumnAttribute ����Property�������С�
* TextAttribute ����Property������TEXT�����С�
* ResultColumnAttribute ����Property�����巵���С�
* IgnoreAttribute ����Property�����Ը����ԡ�
* FieldLengthAttribute ����Property�������г��ȡ�
* DefaultValueAttribute ����Property������Ĭ��ֵ��

#### 2�����ݱ����
##### 2.1����ɾ�Ĳ���


```` csharp
var helper = SqlHelperFactory.OpenSqliteFile(dbFile);
User u = new User() {
    ....
};
helper.Insert(u);
helper.Update(u);
helper.Save(u);
helper.Delete(u);
helper.Update<User>("Set [Name]=@0 WHERE [Id]=@1", "Test", 1);
helper.Delete<User>("WHERE [Id]=@0", 1);
helper.DeleteById<User>(1);
````

##### 2.2����ɾ�Ĳ����¼�

```` csharp
helper.Events.BeforeInsert += Events_BeforeInsert;
helper.Events.AfterInsert += Events_AfterInsert;
helper.Events.BeforeUpdate += Events_BeforeUpdate;
helper.Events.AfterUpdate += Events_AfterUpdate;
helper.Events.BeforeDelete += Events_BeforeDelete;
helper.Events.AfterDelete += Events_AfterDelete;
helper.Events.BeforeExecuteCommand += Events_BeforeExecuteCommand;
helper.Events.AfterExecuteCommand += Events_AfterExecuteCommand;
helper.Events.ExecuteException += Events_ExecuteException;
````

##### 2.3��ʹ������
```` csharp
    using (var tran=helper.UseTransaction()) {
        ...
        tran.Complete();//�ύ Ĭ��
        tran.Abort();//ȡ��
    }
````

#### 3��SQL��ѯ

##### 3.1��������ѯ
```` csharp
var helper = SqlHelperFactory.OpenSqliteFile(dbFile);
var user1 = helper.Single<User>("SELECT * FROM Users where [Id]=@0", 1);
var user2 = helper.SingleById<User>(1);
var user3 = helper.SingleOrDefault<User>("SELECT * FROM Users where [Id]=@0", 1);
var user4 = helper.SingleOrDefaultById<User>(1);

var user5 = helper.First<User>("SELECT * FROM Users where [Id]=@0", 1);
var user6 = helper.FirstOrDefault<User>("SELECT * FROM Users where [Id]=@0", 1);

var dataset = helper.ExecuteDataSet("SELECT * FROM Users where [Id]=@0", 1);
var datatable = helper.ExecuteDataTable("SELECT * FROM Users where [Id]=@0", 1);

var userCount = helper.Count<User>("SELECT COUNT(*) FROM Users Where [UserType]=@0", 1);
var userCount2 = helper.ExecuteScalar<int>("SELECT COUNT(*) FROM Users Where [UserType]=@0", 1);
````


##### 3.2���б��ѯ
```` csharp
var helper = SqlHelperFactory.OpenSqliteFile(dbFile);
var users = helper.Select<User>("SELECT * FROM Users Where [UserType]=@0", 1);
var usersPage = helper.Page <User>(1,20,"SELECT * FROM Users Where [UserType]=@0", 1);
var users2=helper.SkipTake<User>(0,20,"SELECT * FROM Users Where [UserType]=@0", 1);
````

##### 3.3����SQL

```` csharp
var helper = SqlHelperFactory.OpenSqliteFile(dbFile);
var users1 = helper.Select<User>("SELECT * FROM Users Where [UserType]=@0", 1);
var users2 = helper.Select<User>("FROM Users Where [UserType]=@0", 1);
var users3 = helper.Select<User>("Where [UserType]=@0", 1);

helper.Update<User>("UPDATE Users Set [Name]=@0 WHERE [Id]=@1", "Test", 1);
helper.Update<User>("Set [Name]=@0 WHERE [Id]=@1", "Test", 1);

````



##### 3.4�����е�OnLoaded����

```` csharp
public class User
{
    public int Id {get;set;}
    public int UserType {get;set;}
    public string UserName {get;set;}
    public string Password {get;set;}
    public string NickName {get;set;}
    public bool IsLoad {get;set;}

    public void OnLoaded()
    {
        IsLoad=true;
    }
}
var user1 = helper.Single<User>("where [Id]=@0", 1);
Console.WriteLine(user1.IsLoad.ToString());
````


#### 4����̬��ѯ
##### 4.1�������ѯ

```` csharp
public User FindUser(int userId,string userName,string nickName)
{
    var helper = SqlHelperFactory.OpenMysql("127.0.0.1", "web", "root", "123456");
    return helper.CreateWhere<User>()
        .IfTrue(userId > 0).Where((u) => u.Id == userId)
        .IfSet(userName).Where((u) => u.UserName == userName)
        .IfSet(nickName).Where((u) => u.NickName == nickName)
        .SingleOrDefault();
}
````
�����У�
* `IfTrue`��`IfFalse`��`IfSet`��`IfNotSet`��`IfNull`��`IfNotNull`
* `WhereNotIn`��`WhereIn`��`Where`��`OrderBy`��`GroupBy`��`Having`��`SelectColumn`
* `AddNotExistsSql`��`AddExistsSql`��`AddWhereSql`��`AddOrderBySql`��`AddGroupBySql`��`AddHavingSql`��`AddJoinSql`
* `Select`��`Page`��`SkipTake`��`Single`��`SingleOrDefault`��`First`��`FirstOrDefault`��`Count`��`ExecuteDataTable`��`ExecuteDataSet`
* `Select<T>`��`Page<T>`��`SkipTake<T>`��`Single<T>`��`SingleOrDefault<T>`��`First<T>`��`FirstOrDefault<T>`

##### 4.2������ѯ

```` csharp
    var helper = SqlHelperFactory.OpenMysql("127.0.0.1", "web", "root", "123456");
    var list = helper.CreateWhere<User1, UserPay>()
            .On2((u, up) => u.AutoID == up.UserID)
            .Where((u, up) => u.AutoID == 8)
            .Where((u, up) => up.State == 1)
            .Select();
````
���֧��5�����ݱ����÷����ȵ����ѯ��һ��
* `On2`��`On3`��`On4`��`On5`


##### 4.4����������
```` csharp
[Table("Users",fixTag:"Admin")]
public class User
{
    public int Id {get;set;}
    public int UserType {get;set;}
    public string UserName {get;set;}
    public string Password {get;set;}
    public string NickName {get;set;}
    public bool IsLoad {get;set;}

    public void OnLoaded()
    {
        IsLoad=true;
    }
}
helper.TableNameManger.Set("Admin", "Db", "");
var user1 = helper.Single<User>("where [Id]=@0", 1);
// SELECT * FROM DbUsers where [Id]=@0
````

##### 4.4��������SQL
```` csharp
    var helper = SqlHelperFactory.OpenMysql("127.0.0.1", "web", "root", "123456");
    var list1 = helper.CreateWhere<User1, UserPay>()
            .On2((u, up) => u.AutoID == up.UserID)
            .Where((u, up) => u.AutoID == 8)
            .SelectColumn((u, up) => SQL.Sum(up.Money), "TotlePay")
            .SelectColumn((u, up) =>up.UserID)
            .Select<dynamic>();
````

#### 5����̬UPDATE
##### 5.1��һ���򵥵Ķ�̬UPDATE
```` csharp
helper.CreateWhere<User>()
    .Where((u) => u.Id == userId)
    .SetValue((u) => u.NickName = "Test")
    .Update();

var users = helper.CreateWhere<User>()
                .Where((u) => u.Id == userId)
                .SetValue((u) => u.NickName = "Test")
                .UpdateAndSelect();
````

##### 5.2��Update��UpdateAndSelect����
    `Update`��`UpdateAndSelect`���ʵ������ǣ�
    1. `Update`ʹ��SQL���Ը��£�ֻ֧��ֱ�Ӹ�ֵ��
    2. `UpdateAndSelect`��SELECT��Ȼ���ڴ����ִ��Action����UPDATE��

```` csharp
helper.CreateWhere<User>()
    .Where((u) => u.Id == userId)
    .SetValue((u) => u.UserName = "Test")
    .SetValue((u)=>u.NickName=u.NickName+"u")
    .Update();
````
��������Ĵ��룬��ᷢ��`UserName`�޸��ˣ���`NickName`δ�޸ġ�

```` csharp
helper.CreateWhere<User>()
    .Where((u) => u.Id == userId)
    .SetValue((u) => u.UserName = "Test")
    .SetValue((u)=>u.NickName=u.NickName+"u")
    .UpdateAndSelect();
````
��������Ĵ��룬��ᷢ��`UserName`��`NickName`���޸��ˡ�


#### 6���洢����
##### 6.1��������
```` csharp
    public class Chart_GetDeviceCount : SqlProcess
    {
        public Chart_GetDeviceCount(ToolGood.ReadyGo.SqlHelper helper) : base(helper)
        {
        }
        protected override void OnInit()
        {
            _ProcessName = "Chart_GetDeviceCount";

            Add<int>("_AgentId", false);
            Add<int>("_IsAll", false);
            Add<int>("_Type", false);
            Add<DateTime>("_StartDate", false);
            Add<DateTime>("_EndDate", false);

        }
        public int AgentId { get { return _G<int>("_AgentId"); } set { _S("_AgentId", value); } }
        public int IsAll { get { return _G<int>("_IsAll"); } set { _S("_IsAll", value); } }
        public int Type { get { return _G<int>("_Type"); } set { _S("_Type", value); } }
        public DateTime StartDate { get { return _G<DateTime>("_StartDate"); } set { _S("_StartDate", value); } }
        public DateTime EndDate { get { return _G<DateTime>("_EndDate"); } set { _S("_EndDate", value); } }
    }
````

##### 6.2���洢ִ��
```` csharp
    var helper = SqlHelperFactory.OpenMysql("127.0.0.1", "wifi86", "root", "123456");
    Chart_GetDeviceCount c = new Chart_GetDeviceCount(helper);
    c.AgentId = 0;
    c.IsAll = 0;
    c.Type = 0;
    c.StartDate = DateTime.Parse("2016-06-01");
    c.EndDate = DateTime.Parse("2016-07-01");
            
    var dt= c.ExecuteScalar<int> ();
````

#### 7������
##### 7.1��ʹ�û���
```` csharp
    var helper = SqlHelperFactory.OpenMysql("127.0.0.1", "wifi86", "root", "123456");
    var user = helper.UseCache(20,"tag").SingleById<User>(1);
````

##### 7.2���滻������
```` csharp
using ToolGood.ReadyGo.Caches

    public class NullCacheService : ICacheService
    {
        ...
    }
    var helper = SqlHelperFactory.OpenMysql("127.0.0.1", "wifi86", "root", "123456");
    helper.Config.CacheService = new NullCacheService();
    helper.Config.CacheTime = 20;
````


#### 8��ʹ�ô����Ķ�д����
```` csharp
    public class NullCacheService : ICacheService
    {
        ...
    }
    var helper = SqlHelperFactory.OpenMysql("127.0.0.1", "wifi86", "root", "123456");
    helper.Config.CacheService = new NullCacheService();
    helper.Config.CacheTime = 20;
````


#### 9��SQLִ�м��

#### 9.1����һ��SQLִ�����

```` csharp
    var sql = helper.Sql.LastSQL;
    var args = helper.Sql.LastArgs;
    var cmd = helper.Sql.LastCommand;
    var err = helper.Sql.LastErrorMessage;
    var commandTimeout = helper.Sql.LastCommandTimeout;

    var isUsed = helper.Sql.LastUsedCacheService;
    var cacheService = helper.Sql.LastCacheService;
    var cacheTime = helper.Sql.LastCacheTime;
    var cacheTag = helper.Sql.LastCacheTag;
````



#### 9.2���鿴���
```` csharp
using ToolGood.ReadyGo.Monitor

var sqlMonitor = helper.Sql.SqlMonitor;
var html = sqlMonitor.ToHtml();
var text = sqlMonitor.ToText();
````



#### 9.3���滻�����

```` csharp
using ToolGood.ReadyGo.Monitor

public class NullSqlMonitor : ISqlMonitor
{
    ...
}
helper.Config.SqlMonitor = new NullSqlMonitor();
````




#### 10���ٶȶԱ�

<table>
	<tr>
		<th>����</th>
		<th>����ʱ��</th>		
		<th>��ע</th>
	</tr>
	<tr>
		<td>Linq 2 SQL </td>
		<td>81ms</td>
		<td>Not super typical involves complex code</td>
	</tr>
	
</table>

��д

PetaPoco

ToolGood.ReadyGo

ToolGood.ReadyGo ��̬����


----------------------------
