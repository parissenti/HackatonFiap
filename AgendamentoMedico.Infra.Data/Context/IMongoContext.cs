﻿using AgendamentoMedico.Domain.Entitites;
using MongoDB.Driver;

namespace AgendamentoMedico.Infra.Data.Context
{
    public interface IMongoContext
    {
        Task<IClientSessionHandle> StartSessionAsync();
        IMongoCollection<Usuario> Usuarios { get; }
        IMongoCollection<PeriodoAtendimento> PeriodoAtendimentos { get; }
        IMongoCollection<ConsultaAgendamento> ConsultaAgendamentos { get; }
    }
}

