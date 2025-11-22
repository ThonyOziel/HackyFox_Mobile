[System.Serializable]
public class Usuario
{
    public int id_usuario;
    public string correo;
    public string contraseña;
    public string fecha_registro;

    public Usuario(int id, string c, string pass, string fecha)
    {
        id_usuario = id;
        correo = c;
        contraseña = pass;
        fecha_registro = fecha;
    }
}

