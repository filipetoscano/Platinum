declare @a int;
declare @b int;

set @a = 1;
set @b = 2;
--//
begin
    set nocount on;

    select 5+@a A, getutcdate() Moment;

    select 9+@b B
    union all
    select 13+@b B;
end;
/* eof */