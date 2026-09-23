using System;
using System.Collections.Generic;
using System.Text;

namespace THMTecnologicalSolutions.Portfolio.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; }

    public bool IsActive { get; protected set; }

    public DateTime CreatedAt { get; protected set; }

    public DateTime? UpdatedAt { get; protected set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    protected void SetUpdatedAt()
        => UpdatedAt = DateTime.UtcNow;

    public void Activate()
    {
        if (IsActive)
            return;

        IsActive = true;
        SetUpdatedAt();
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;
        SetUpdatedAt();
    }
}