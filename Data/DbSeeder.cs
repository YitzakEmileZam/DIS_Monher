using DIS_Monher.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DIS_Monher.Data;

public static class DbSeeder
{
    public const string RolAdmin = "Admin";
    public const string RolCliente = "Cliente";

    public static async Task SeedAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();

        // Roles
        var roles = services.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var r in new[] { RolAdmin, RolCliente })
            if (!await roles.RoleExistsAsync(r))
                await roles.CreateAsync(new IdentityRole(r));

        // Usuarios de prueba
        var users = services.GetRequiredService<UserManager<IdentityUser>>();
        await CrearUsuario(users, "admin@monher.com", "Admin123!", RolAdmin);
        await CrearUsuario(users, "cliente@monher.com", "Cliente123!", RolCliente);

        // Materiales (vista admin)
        if (!await db.Materiales.AnyAsync())
        {
            var hoy = DateTime.Now;
            db.Materiales.AddRange(
                M("MAT-001", "Tablero de roble 18mm", "Maderas", "plancha", 120, 40, hoy.AddDays(-20)),
                M("MAT-002", "Tablero MDF 15mm", "Maderas", "plancha", 35, 50, hoy.AddDays(-15)),
                M("MAT-003", "Tornillos 2\" (caja x100)", "Ferretería", "caja", 60, 20, hoy.AddDays(-12)),
                M("MAT-004", "Bisagras de cierre suave", "Ferretería", "unid", 48, 100, hoy.AddDays(-9)),
                M("MAT-005", "Barniz poliuretano", "Acabados", "galón", 25, 10, hoy.AddDays(-7)),
                M("MAT-006", "Espuma de alta densidad", "Tapicería", "m2", 90, 30, hoy.AddDays(-5)),
                M("MAT-007", "Tela de tapiz gris", "Tapicería", "metro", 12, 40, hoy.AddDays(-3)),
                M("MAT-008", "Pegamento de contacto", "Acabados", "galón", 30, 8, hoy.AddDays(-1))
            );
        }

        // Productos terminados (catálogo del cliente)
        if (!await db.Productos.AnyAsync())
        {
            db.Productos.AddRange(
                P("MUE-001", "Cama Doble 1.40m", "Dormitorio", "Roble Natural", 1199, 15, "Cabecera lisa y base reforzada."),
                P("MUE-002", "Cama Queen 1.60m", "Dormitorio", "Nogal", 1490, 8, "Cabecera tapizada con espacio para almacenaje."),
                P("MUE-003", "Ropero 3 puertas", "Dormitorio", "Blanco Mate", 980, 4, "Bisagras de cierre suave y cajonera interna."),
                P("MUE-004", "Mesa de noche", "Dormitorio", "Roble Natural", 280, 22, "Un cajón y repisa inferior."),
                P("MUE-005", "Mesa Comedor 6 plazas", "Comedor", "Nogal Oscuro", 1250, 3, "Tablero de 1.80m con patas torneadas."),
                P("MUE-006", "Silla de comedor", "Comedor", "Nogal / tapiz gris", 210, 40, "Asiento acolchado y respaldo curvo."),
                P("MUE-007", "Sofá Modular 3 plazas", "Sala", "Gris", 1890, 0, "Módulos desmontables con funda lavable."),
                P("MUE-008", "Mesa de centro", "Sala", "Roble Natural", 450, 11, "Tablero con repisa inferior."),
                P("MUE-009", "Escritorio Ejecutivo 1.40m", "Oficina", "Wengué", 760, 9, "Pasacables y dos cajones con llave."),
                P("MUE-010", "Silla ergonómica", "Oficina", "Negro", 540, 14, "Soporte lumbar y altura regulable.")
            );
        }

        await db.SaveChangesAsync();
    }

    private static async Task CrearUsuario(UserManager<IdentityUser> users, string email, string pass, string rol)
    {
        var u = await users.FindByEmailAsync(email);
        if (u == null)
        {
            u = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
            var r = await users.CreateAsync(u, pass);
            if (!r.Succeeded) return;
        }
        if (!await users.IsInRoleAsync(u, rol))
            await users.AddToRoleAsync(u, rol);
    }

    private static Material M(string cod, string nom, string cat, string um, int stock, int min, DateTime fecha) => new()
    {
        Codigo = cod, Nombre = nom, Categoria = cat, UnidadMedida = um,
        StockActual = stock, StockMinimo = min, FechaIngreso = fecha,
        UsuarioResponsable = "admin@monher.com"
    };

    private static Producto P(string cod, string nom, string cat, string acabado, decimal precio, int stock, string desc) => new()
    {
        Codigo = cod, Nombre = nom, Categoria = cat, Acabado = acabado,
        Precio = precio, Stock = stock, Descripcion = desc
    };
}
