use dsa_lims
go

alter table assignment_preparation_method add use_existing_preparation bit default 0 not null;
go

update apm
set use_existing_preparation = 1 
from assignment_preparation_method as apm
	inner join assignment_sample_type as ast on apm.assignment_sample_type_id = ast.id
	inner join assignment as a on ast.assignment_id = a.id
where a.laboratory_id <> apm.preparation_laboratory_id
go

ALTER proc csp_insert_assignment_preparation_method
	@id uniqueidentifier,	
	@assignment_sample_type_id uniqueidentifier,	
	@preparation_method_id uniqueidentifier,
	@preparation_method_count int,
	@preparation_laboratory_id uniqueidentifier,
	@comment nvarchar(1000),
	@create_date datetime,
	@create_id uniqueidentifier,
	@update_date datetime,
	@update_id uniqueidentifier,
	@use_existing_preparation bit
as	
	insert into assignment_preparation_method values (
		@id,	
		@assignment_sample_type_id,	
		@preparation_method_id,
		@preparation_method_count,
		@preparation_laboratory_id,
		@comment,
		@create_date,
		@create_id,
		@update_date,
		@update_id,
		@use_existing_preparation
	);
GO

ALTER proc csp_update_assignment_preparation_method
	@id uniqueidentifier,	
	@assignment_sample_type_id uniqueidentifier,	
	@preparation_method_id uniqueidentifier,
	@preparation_method_count int,
	@preparation_laboratory_id uniqueidentifier,
	@comment nvarchar(1000),	
	@update_date datetime,
	@update_id uniqueidentifier,
	@use_existing_preparation bit
as	
	update assignment_preparation_method set		
		assignment_sample_type_id = @assignment_sample_type_id,	
		preparation_method_id = @preparation_method_id,
		preparation_method_count = @preparation_method_count,
		preparation_laboratory_id = @preparation_laboratory_id,
		comment = @comment,		
		update_date = @update_date,
		update_id = @update_id,
		use_existing_preparation = @use_existing_preparation
	where id = @id
GO


update counters set value = 3 where name like 'database_version'
go