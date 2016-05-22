
select top 1 ActivityId, ExecutionId, Action, Step, XmlMessage, Moment
from WCF_JOURNAL
order by Moment desc

/* eof */