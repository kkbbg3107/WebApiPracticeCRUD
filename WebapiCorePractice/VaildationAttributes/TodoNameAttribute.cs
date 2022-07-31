using System.ComponentModel.DataAnnotations;
using WebapiCorePractice.DTOs;
using WebapiCorePractice.Models;

namespace WebapiCorePractice.VaildationAttribute
{
    public class TodoNameAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // 取得di服務
            TodoContext _todoContext = (TodoContext)validationContext.GetService(typeof(TodoContext));

            var name = (string)value;

            var findName = _todoContext.Players.Where(x => x.Name == name).Select(x => x);

            var dto = validationContext.ObjectInstance;

            // 更新時更新非掛attr的屬性時
            if (dto.GetType() == typeof(PlayerPutDto))
            {
                var dtoUpdate = (PlayerPutDto)dto;
                findName = findName.Where(x => x.Id != dtoUpdate.Id);
            }

            if (findName.FirstOrDefault() != null)
            {
                return new ValidationResult("已存在相同名稱");
            }

            return ValidationResult.Success;
        }
       
    }
}
