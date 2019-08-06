﻿namespace Tests.Configuration
{
	/// <summary>
	/// Our tests can run in the following modes. Depending on which mode is selected
	/// some tests won't be discovered. This is not the same as skipping as undiscovered tests are not reported.
	/// </summary>
	public enum TestMode
	{
		/// <summary> Only run unit tests</summary>
		Unit,
		/// <summary> Only run integration tests </summary>
		Integration,
		/// <summary> Run both unit and integration test, due note not all classes are written with this mode in mind </summary>
		Mixed
	}
}
