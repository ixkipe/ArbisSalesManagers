namespace AmoAsterisk.ApiManagement.ResponseModels;

#nullable disable warnings
public class ContactJsonResponseModel {
  public int _Page { get; set; }
  public Links _links { get; set; }
  public EmbeddedContacts _embedded { get; set; }
}

public class EmbeddedContacts {
  public ContactModel[] Contacts { get; set; }
}

public class ContactModel {
  public int Id { get; set; }
  public string Name { get; set; }
  public string First_Name { get; set; }
  public string Last_Name { get; set; }
  public int Responsible_User_Id { get; set; }
  public int Group_Id { get; set; }
  public int Created_By { get; set; }
  public int Updated_By { get; set; }
  public long Created_At { get; set; }
  public long Updated_At { get; set; }
  public long? Closest_Task_At { get; set; }
  public bool Is_Deleted { get; set; }
  public bool Is_Unsorted { get; set; }
  public CustomField[] Custom_Fields_Values { get; set; }
  public int Account_Id { get; set; }
  public Links _links { get; set; }
  public MiscEmbedded _embedded { get; set; }
}

public class CustomField {
  public int Field_Id { get; set; }
  public string Field_Name { get; set; }
  public string Field_Code { get; set; }
  public string Field_Type { get; set; }
  public FieldValue[] Values { get; set; }
}

public class FieldValue {
  public string Value { get; set; }
  public int Enum_Id { get; set; }
  public string Enum_Code { get; set; }
}

public class MiscEmbedded {
  public Tag[] Tags { get; set; }
  public Company[] Companies { get; set; }
}

public class Tag {
  public int Id { get; set; }
  public string Name { get; set; }
  public string? Color { get; set; }
}

public class Company {
  public int Id { get; set; }
  public Links _links { get; set; }
}