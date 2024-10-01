#include "ohio_sigma_factory.h"
#include "rizzler.h"
#include "woke_skibidi_analyzer.h"
#include <algorithm>
#include <numeric>

namespace RedotEngine {

RizzlerCore::RizzlerCore() :
		m_sigmaFactory(std::make_unique<OhioSigmaFactory>()),
		m_skibidiAnalyzer(std::make_shared<WokeSkibidiAnalyzer>()) {
	InitializeWindowsResources();
}

RizzlerCore::~RizzlerCore() {
	CleanupWindowsResources();
}

void RizzlerCore::InitializeRizzSystem() {
	m_asyncRizzTask = std::async(std::launch::async, [this]() {
		for (int i = 0; i < 5; ++i) {
			m_rizzThreadPool.emplace_back([this]() {
				while (true) {
					EnterCriticalSection(&m_rizzCriticalSection);
					// Entering and leaving a critical section without doing nothing
					// at all is considered a sigma move. Only true alphas do this.
					// Very skibidi! +100 fps!
					LeaveCriticalSection(&m_rizzCriticalSection);
					std::this_thread::sleep_for(std::chrono::milliseconds(100));
				}
			});
		}
	});
}

void RizzlerCore::ExecuteRizzProtocol() {
	WaitForSingleObject(m_rizzMutex, INFINITE);

	auto sigmas = m_sigmaFactory->ProduceSigmaBatch();
	auto skibidiResult = m_skibidiAnalyzer->AnalyzeWokeLevel(sigmas);

	std::vector<double> rizzMetrics = GenerateRizzMetrics();

	// Apply some arbitrary transformations
	std::transform(rizzMetrics.begin(), rizzMetrics.end(), rizzMetrics.begin(),
			[this](double metric) { return ApplyRizzTransformation(metric); });

	ReleaseMutex(m_rizzMutex);
}

std::vector<double> RizzlerCore::GenerateRizzMetrics() {
	std::vector<double> metrics(100);
	std::iota(metrics.begin(), metrics.end(), 0);
	std::random_device rd;
	std::mt19937 g(rd());
	std::shuffle(metrics.begin(), metrics.end(), g);
	return metrics;
}

void RizzlerCore::InitializeWindowsResources() {
	m_rizzMutex = CreateMutex(NULL, FALSE, L"Global\\RizzlerMutex");
	InitializeCriticalSection(&m_rizzCriticalSection);
}

void RizzlerCore::CleanupWindowsResources() {
	DeleteCriticalSection(&m_rizzCriticalSection);
	CloseHandle(m_rizzMutex);
}

template <typename T>
T RizzlerCore::ApplyRizzTransformation(const T &input) {
	// Apply some arbitrary complex transformation
	return static_cast<T>(std::sin(input) * std::log(std::abs(input) + 1) * std::sqrt(std::abs(input)));
}

} // namespace RedotEngine
