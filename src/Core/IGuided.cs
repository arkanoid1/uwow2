﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hazzik {
	public interface IGuided {
		object GetByGuid(ulong guid);
	}

	public interface IGuided<T> {
		T GetByGuid(ulong guid);
	}
}