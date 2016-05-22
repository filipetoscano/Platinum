declare @a int;
declare @b int;

set @a = 1;
set @b = 2;
--//
begin
    set nocount on;

    select 2*@a A, getutcdate() Moment;

    select 4*@b B
    union all
    select 8*@b B;

end;
/* eof */