namespace AmoAsterisk.ApiManagement.ResponseModels;

#nullable disable warnings
public class UserJsonResponseModel {
  public int _Total_Items { get; set; }
  public int _Page { get; set; }
  public int _Page_Count { get; set; }
  public Links _links { get; set; }
  public EmbeddedUsers _embedded { get; set; }
}

public class EmbeddedUsers {
  public UserModel[] Users { get; set; }
}

public class UserModel {
  public int Id { get; set; }
  public string Name { get; set; }
  public string Email { get; set; }
  public string Lang { get; set; }
  public Rights MyProperty { get; set; }
  public Links _links { get; set; }
}

public class Privileged {
  public string View { get; set; }
  public string Edit { get; set; }
  public string Add { get; set; }
  public string Delete { get; set; }
  public string Export { get; set; }
}

public class Rights {
  public Leads Leads { get; set; }
  public Contacts Contacts { get; set; }
  public Companies Companies { get; set; }
  public Tasks Tasks { get; set; }
  public bool Mail_Access { get; set; }
  public bool Catalog_Access { get; set; }
  public bool Files_Access { get; set; }
  public StatusRights[] Status_Rights { get; set; }
  public bool Is_Admin { get; set; }
  public bool Is_Free { get; set; }
  public bool Is_Active { get; set; }
  public int? Group_Id { get; set; }
  public int? Role_Id { get; set; }
}

public class Leads : Privileged {}
public class Contacts : Privileged {}

public class Companies : Privileged {}

public class Tasks {
  public string Edit { get; set; }
  public string Delete { get; set; }
}

public class StatusRights {
  public string Entity_Type { get; set; }
  public int Pipeline_Id { get; set; }
  public int Status_Id { get; set; }
  public Rights Rights { get; set; }
}

public class Links {
  public Self? Self { get; set; }
  public Next? Next { get; set; }
}

public class Linkable {
  public string href { get; set; }
}
public class Self : Linkable {}
public class Next : Linkable {}
