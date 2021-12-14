/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     9/12/2021 21:30:19                           */
/*==============================================================*/
USE PrestamosDb
GO

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('CUOTA_PRESTAMO') and o.name = 'FK_CUOTA_PR_REFERENCE_DESCRIPC')
alter table CUOTA_PRESTAMO
   drop constraint FK_CUOTA_PR_REFERENCE_DESCRIPC
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('LOG_REGISTRO') and o.name = 'FK_LOG_REGI_RELATIONS_CUOTA_PR')
alter table LOG_REGISTRO
   drop constraint FK_LOG_REGI_RELATIONS_CUOTA_PR
go

if exists (select 1
            from  sysobjects
           where  id = object_id('CUOTA_PRESTAMO')
            and   type = 'U')
   drop table CUOTA_PRESTAMO
go

if exists (select 1
            from  sysobjects
           where  id = object_id('DESCRIPCION_POR_VALOR')
            and   type = 'U')
   drop table DESCRIPCION_POR_VALOR
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('LOG_REGISTRO')
            and   name  = 'RELATIONSHIP_1_FK'
            and   indid > 0
            and   indid < 255)
   drop index LOG_REGISTRO.RELATIONSHIP_1_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('LOG_REGISTRO')
            and   type = 'U')
   drop table LOG_REGISTRO
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TASAS_POR_EDAD')
            and   type = 'U')
   drop table TASAS_POR_EDAD
go

/*==============================================================*/
/* Table: CUOTA_PRESTAMO                                        */
/*==============================================================*/
create table CUOTA_PRESTAMO (
   FECHAPRESTAMO        datetime             not null,
   MONTOPRESTAMO        decimal(38,2)        not null,
   IDPRESTAMO           int                  identity,
   FKMESES              int                  null,
   constraint PK_CUOTA_PRESTAMO primary key (IDPRESTAMO)
)
go

/*==============================================================*/
/* Table: DESCRIPCION_POR_VALOR                                 */
/*==============================================================*/
create table DESCRIPCION_POR_VALOR (
   DESCRIPCION_VALOR    char(300)            not null,
   VALOR_VALOR          int                  not null,
   ID_VALOR             int                  identity,
   constraint PK_DESCRIPCION_POR_VALOR primary key (ID_VALOR)
)
go

/*==============================================================*/
/* Table: LOG_REGISTRO                                          */
/*==============================================================*/
create table LOG_REGISTRO (
   IDCONSULTA           int                  identity,
   IDPRESTAMO           int                  null,
   FECHACONSULTA        datetime             not null,
   EDADCONSULTA         int                  not null,
   IPCONSULTA         char(16)             not null,
   VALORCONSULTA         decimal(38,2)        not null,
   constraint PK_LOG_REGISTRO primary key (IDCONSULTA)
)
go

/*==============================================================*/
/* Index: RELATIONSHIP_1_FK                                     */
/*==============================================================*/
create index RELATIONSHIP_1_FK on LOG_REGISTRO (
IDPRESTAMO ASC
)
go

/*==============================================================*/
/* Table: TASAS_POR_EDAD                                        */
/*==============================================================*/
create table TASAS_POR_EDAD (
   EDAD_TASAS           int                  not null,
   TASA_TASAS           decimal(10,2)        null,
   ID_TASAS             int                  identity,
   constraint PK_TASAS_POR_EDAD primary key (ID_TASAS)
)
go

alter table CUOTA_PRESTAMO
   add constraint FK_CUOTA_PR_REFERENCE_DESCRIPC foreign key (FKMESES)
      references DESCRIPCION_POR_VALOR (ID_VALOR)
go

alter table LOG_REGISTRO
   add constraint FK_LOG_REGI_RELATIONS_CUOTA_PR foreign key (IDPRESTAMO)
      references CUOTA_PRESTAMO (IDPRESTAMO)
go

