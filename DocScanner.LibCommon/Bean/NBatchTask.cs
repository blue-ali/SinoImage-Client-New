using System;

namespace DocScanner.Bean
{
	public class NBatchTask
	{
		public NBatchInfo Batch
		{
			get;
			set;
		}

		public int Serial
		{
			get;
			set;
		}

		public DateTime StartTime
		{
			get;
			set;
		}

		public NBatchTaskStatus Status
		{
			get;
			set;
		}
	}
}
