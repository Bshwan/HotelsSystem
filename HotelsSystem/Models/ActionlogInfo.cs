namespace HotelsSystem.Models
{
    public class ActionlogInfo
    {
        public int actionlog_ID { get; set; }
        public int UserTypeID { get; set; }
        public string? UserTypeName { get; set; }
        public int actionlog_ActionType { get; set; }
        public int UserID { get; set; }
        public string? actionlog_UserName { get; set; }
        public string? actionlogtype_Name { get; set; }
        public string? actionlog_TableName { get; set; }
        public string? actionlog_FieldName { get; set; }
        public string? actionlog_Value { get; set; }
        public string? actionlog_OldValue { get; set; }
        public DateTime? actionlog_EntryDate { get; set; }
        public DateTime? actionlog_EntryDate2 { get; set; }
        public int actionlog_ProfileID { get; set; }
    }
}
