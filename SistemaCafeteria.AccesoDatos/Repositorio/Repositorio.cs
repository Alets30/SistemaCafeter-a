using Microsoft.EntityFrameworkCore;
using SistemaCafeteria.AccesoDatos.Data;
using SistemaCafeteria.AccesoDatos.Repositorio.IRepositorio;
using SistemaCafeteria.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCafeteria.AccesoDatos.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        //Acceso a los datos es decir la conexion de la DB 
        private readonly ApplicationDbContext _db;



        //Necesitamos mandar el conjunto de datos a travez de una variable
        internal DbSet<T> dbSet;
        private ApplicationDbContext db;

        public Repositorio(ApplicationDbContext db, DbSet<T> dbSet)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public Repositorio(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<T> Obtener(int id)
        {
            return await dbSet.FindAsync(id); //Select * from where id = id
        }

        public async Task<IEnumerable<T>> ObtenerTodos(Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>, IOrderedQueryable<T>> OrdeBy = null, string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filtro == null)
            {
                query = query.Where(filtro); //Select * from table
            }

            if (incluirPropiedades != null)
            {
                foreach (var incluirProp in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp); //Marca, Categoria
                }
            }

            if (OrdeBy != null)
            {
                query = OrdeBy(query);
            }
            if (isTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync();

        }

        public async Task<T> ObtenerPrimero(Expression<Func<T, bool>> filtro = null, string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filtro == null)
            {
                query = query.Where(filtro); //Select * from table
            }

            if (incluirPropiedades != null)
            {
                foreach (var incluirProp in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp); //Marca, Categoria
                }
            }


            if (isTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task Agregar(T entidad)
        {
            await dbSet.AddAsync(entidad);
        }

        public void Remover(T entidad)
        {
            dbSet.Remove(entidad); //Delete
        }



        public void RemoverRango(IEnumerable<T> entidad)
        {
            dbSet.RemoveRange(entidad);
        }
    }
