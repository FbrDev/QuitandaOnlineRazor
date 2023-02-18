using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using QuitandaOnline.Models;

namespace QuitandaOnline.Pages
{
    public class ConfirmacaoEmailModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;

        public ConfirmacaoEmailModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public string StatusMessage { get; set; }
        public bool EmailConfirmado { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToPage("/Index");
            }

            var usuario = await _userManager.FindByIdAsync(userId);
            if (usuario == null)
            {
                return NotFound($"N�o foi poss�vel encontrar o usu�rio com ID '{userId}'.");
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(usuario, token);
            EmailConfirmado = result.Succeeded;
            StatusMessage = EmailConfirmado ? "E-mail confirmado com sucesso!" :
                "Ocorreu um erro ao confirmar seu e-mail.";
            return Page();
        }
    }
}