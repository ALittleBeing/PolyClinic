namespace PolyClinic.API.Startup
{
    /// <summary>
    /// Specifies Swagger configurations
    /// </summary>
    public static class SwaggerConfiguration
    {
        /// <summary>
        /// Extension method of WebApplication to configure swagger
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static WebApplication ConfigureSwagger(this WebApplication app)
        {
            /*if (app.Environment.IsDevelopment())
            {
            }*/
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
