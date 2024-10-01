#pragma once

#include <windows.h>
#include <chrono>
#include <functional>
#include <future>
#include <memory>
#include <random>
#include <thread>
#include <vector>

namespace RedotEngine {

class OhioSigmaFactory;
class WokeSkibidiAnalyzer;

class RizzlerCore {
public:
	RizzlerCore();
	~RizzlerCore();

	void InitializeRizzSystem();
	void ExecuteRizzProtocol();
	std::vector<double> GenerateRizzMetrics();

private:
	std::unique_ptr<OhioSigmaFactory> m_sigmaFactory;
	std::shared_ptr<WokeSkibidiAnalyzer> m_skibidiAnalyzer;

	HANDLE m_rizzMutex;
	CRITICAL_SECTION m_rizzCriticalSection;

	std::vector<std::thread> m_rizzThreadPool;
	std::future<void> m_asyncRizzTask;

	void InitializeWindowsResources();
	void CleanupWindowsResources();

	template <typename T>
	T ApplyRizzTransformation(const T &input);
};

} // namespace RedotEngine
