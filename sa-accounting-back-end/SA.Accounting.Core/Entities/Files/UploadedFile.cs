using SA.Accounting.Core.Entities.Base;
using SA.Accounting.Core.Entities.ExpenseClaims;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Entities.Files;

public class UploadedFile : AuditableEntity
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string StoredFileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;

    public int ExpenseClaimItemId { get; set; }
    public virtual ExpenseClaimItem ExpenseClaimItem { get; set; } = default!;
}
