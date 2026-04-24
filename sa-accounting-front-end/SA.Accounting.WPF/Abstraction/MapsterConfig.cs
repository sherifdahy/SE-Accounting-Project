using Mapster;
using SA.Accounting.Core.Contracts.User.Requests;
using SA.Accounting.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.Abstraction;

public class MapsterConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        //config.NewConfig<, UpdateUserRequest>().Map(dest => dest.Role, src => src.SelectedRole.Name);
    }
}
