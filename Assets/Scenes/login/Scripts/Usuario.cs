[System.Serializable]
public class Usuario
{
    public int id_usuario;
    public string correo;
    public string contraseña;
    public string fecha_registro;
    public string nombre_mascota;

    public Usuario(int id, string correo, string pass, string fecha, string mascota = "")
    {
        this.id_usuario = id;
        this.correo = correo;
        this.contraseña = pass;
        this.fecha_registro = fecha;
        this.nombre_mascota = mascota;
    }
}

