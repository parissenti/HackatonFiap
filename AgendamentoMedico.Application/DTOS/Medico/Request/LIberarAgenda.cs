﻿namespace AgendamentoMedico.Application.DTOS.Medico.Request
{
    public class LiberarAgenda
    {
        public Guid idMedico { get; set; }
        public DateTime dataLiberar { get; set; }
    }
}
